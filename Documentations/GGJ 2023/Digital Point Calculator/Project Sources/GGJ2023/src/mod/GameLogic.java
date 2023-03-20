package mod;

import java.beans.PropertyChangeEvent;
import java.beans.PropertyChangeListener;
import java.util.ArrayList;
import java.util.List;
import java.util.Random;

public class GameLogic {
    
    public static final String PROP_STAGE_CHANGED = "stageChanged";
    
    private static final GameLogic instance = new GameLogic();
    
    private final List<Player> players = new ArrayList<>();
    private final List<Event> events = new ArrayList<>();
    private final Random random = new Random();
    
    private PropertyChangeListener stageListener = null;
    private Stage currStage = Stage.SETUP;
    private Event currEvent = null;
    private int harvester = 0;
    
    public static GameLogic getInstance()
    {
        return instance;
    }
    
    public void setStageListener(PropertyChangeListener stageListener) 
    {
        this.stageListener = stageListener;
    }
    
    private void setupEvents()
    {
        events.clear();
        events.add(Event.MOLECRICKET);
        events.add(Event.MOLECRICKET);
        events.add(Event.DROUGHT);
        events.add(Event.DROUGHT);
        events.add(Event.FROST);
        events.add(Event.FROST);
    }
    
    public void setCurrEvent(Event event) 
    {
        if (event == null)
        {
            currEvent = events.remove(random.nextInt(events.size()));
        }
        else
        {
            currEvent = event;
        }
    }
    
    public Event getCurrEvent() 
    {
        return currEvent;
    }
    
    public void addPlayer(Player player)
    {
        players.add(player);
    }

    public void removePlayer(Player player)
    {
        players.remove(player);
    }
    
    public List<Player> getPlayers() 
    {
        return players;
    }

    public Player getHarvestingPlayer() 
    {
        if (harvester >= players.size())
        {
            harvester = 0;
            return null;
        }
        
        return players.get(harvester++);
    }
    
    public void nextStage() 
    {
        Stage lastStage = currStage;
        
        switch (currStage) 
        {
            case SETUP -> {
                currStage = Stage.ROUND1;
                setupEvents();
            }
            case ROUND1 -> currStage = Stage.HARVEST1;
            case HARVEST1 -> {
                currStage = Stage.ROUND2;
            }
            case ROUND2 -> currStage = Stage.HARVEST2;
            case HARVEST2 -> {
                currStage = Stage.ROUND3;
            }
            case ROUND3 -> currStage = Stage.HARVEST3;
            case HARVEST3 -> currStage = Stage.SETUP;
        }
        
        stageListener.propertyChange(new PropertyChangeEvent(this, PROP_STAGE_CHANGED, lastStage, currStage));
    }

    public Stage getStage() 
    {
        return currStage;
    }

    public enum Stage
    {
        SETUP, 
        ROUND1, HARVEST1, 
        ROUND2, HARVEST2, 
        ROUND3, HARVEST3
    }
}
