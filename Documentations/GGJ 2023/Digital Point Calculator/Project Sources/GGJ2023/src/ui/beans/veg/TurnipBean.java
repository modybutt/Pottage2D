package ui.beans.veg;

import static mod.Event.DROUGHT;
import static mod.Event.FROST;
import mod.GameLogic;

public class TurnipBean extends javax.swing.JPanel {

    public TurnipBean() {
        initComponents();
    }

    public int getPoints(boolean glasshouse)
    {
        int points = 0;
        int bonus = 0;
        
        if (calcBean1.getCount() > 0 && calcBean2.getCount() > 0)
        {
            if (calcBean1.getCount() > calcBean2.getCount())
            {
                points = calcBean2.getCount();
            }
            else
            {
                points = calcBean1.getCount();
            }
        }
        
        if (glasshouse == false)
        {
            switch (GameLogic.getInstance().getCurrEvent())
            {
                case DROUGHT -> bonus = 0;
                case FROST -> bonus = points;
            }
        }
        
        points *= 3;
        
        return points + bonus; 
    }
    
    public int getAmount()
    {
        if (calcBean1.getCount() > 0 && calcBean2.getCount() > 0)
        {
            if (calcBean1.getCount() > calcBean2.getCount())
            {
                return calcBean2.getCount();
            }
            else
            {
                return calcBean1.getCount();
            }
        }
        
        return 0;
    }
    
    /**
     * This method is called from within the constructor to initialize the form.
     * WARNING: Do NOT modify this code. The content of this method is always
     * regenerated by the Form Editor.
     */
    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {
        java.awt.GridBagConstraints gridBagConstraints;

        calcBean1 = new ui.beans.VegBean();
        calcBean2 = new ui.beans.VegBean();

        setLayout(new java.awt.GridBagLayout());

        calcBean1.setVegetableName("Turnip [1/2]");
        calcBean1.setVegetablePath("/res/turnip_1.png");
        add(calcBean1, new java.awt.GridBagConstraints());

        calcBean2.setVegetableName("Turnip [2/2]");
        calcBean2.setVegetablePath("/res/turnip_2.png");
        gridBagConstraints = new java.awt.GridBagConstraints();
        gridBagConstraints.gridx = 0;
        gridBagConstraints.gridy = 1;
        add(calcBean2, gridBagConstraints);
    }// </editor-fold>//GEN-END:initComponents


    // Variables declaration - do not modify//GEN-BEGIN:variables
    private ui.beans.VegBean calcBean1;
    private ui.beans.VegBean calcBean2;
    // End of variables declaration//GEN-END:variables
}
