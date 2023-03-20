package mod;

import java.util.HashMap;
import java.util.Map;
import ui.beans.veg.CarrotBean;
import ui.beans.veg.CeleryBean;
import ui.beans.veg.GingerBean;
import ui.beans.veg.ParsnipBean;
import ui.beans.veg.PotatoBean;
import ui.beans.veg.RadishBean;
import ui.beans.veg.BeetRootBean;
import ui.beans.veg.TurnipBean;

public class Player {
    private final Map<Class<?>, Integer> basket = new HashMap<>();
    
    private final String nickname;
    private int points;
    
    public Player(String nickname)
    {
        this.nickname = nickname;
    }

    public String getNickname()
    {
        return nickname;
    }
    
    public int getPoints() {
        return points;
    }

    public void setPoints(int points) {
        this.points = points;
    }

    public void addToBasket(Class<?> type, int amount) 
    {
        Integer stored = basket.get(type);
        
        if (stored == null)
        {
            basket.put(type, amount);
        }
        else
        {
            basket.replace(type, stored + amount);
        }
    }
    
    public void applyCombo()
    {
        int diff = 0;
        int combo = 0;
        int amount;
        
        amount = basket.get(PotatoBean.class);
        if (amount > 0)
        {
            diff++;
                
            if (amount >= 3)
            {
                combo += 3;
            }
        }
        
        amount = basket.get(RadishBean.class);
        if (amount > 0)
        {
            diff++;
            if (amount >= 3)
            {
                combo += 3;
            }
        }
        
        amount = basket.get(GingerBean.class);
        if (amount > 0)
        {
            diff++;
            
            if (amount >= 2)
            {
                combo += 4;
            }
        }
        
        amount = basket.get(BeetRootBean.class);
        if (amount > 0)
        {
            diff++;
        }
        
        amount = basket.get(TurnipBean.class);
        if (amount > 0)
        {
            diff++;
        }
        
        amount = basket.get(CeleryBean.class);
        if (amount > 0)
        {
            diff++;
        }
        
        amount = basket.get(ParsnipBean.class);
        if (amount > 0)
        {
            diff++;
        }
        
        amount = basket.get(CarrotBean.class);
        if (amount > 0)
        {
            diff++;
        }
        
        if (diff >= 4)
        {
            combo += 5;
        }
        else if (diff >= 3)
        {
            combo += 3;
        }
        
        setPoints(getPoints() + combo);
        basket.clear();
    }
}
