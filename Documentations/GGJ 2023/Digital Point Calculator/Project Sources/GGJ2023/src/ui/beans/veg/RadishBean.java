package ui.beans.veg;

import static mod.Event.DROUGHT;
import static mod.Event.FROST;
import mod.GameLogic;

public class RadishBean extends javax.swing.JPanel {

    public RadishBean() {
        initComponents();
    }

    public int getPoints(boolean glasshouse)
    {
        int points = calcBean1.getCount();
        int bonus = 0;
        
        if (glasshouse == false)
        {
            switch (GameLogic.getInstance().getCurrEvent())
            {
                case DROUGHT -> bonus = points;
                case FROST -> bonus = -points;
            }
        }
        
        return points + bonus; 
    }
    
    public int getAmount()
    {
        return calcBean1.getCount();
    }
    
    /**
     * This method is called from within the constructor to initialize the form.
     * WARNING: Do NOT modify this code. The content of this method is always
     * regenerated by the Form Editor.
     */
    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {

        calcBean1 = new ui.beans.VegBean();

        setLayout(new java.awt.GridBagLayout());

        calcBean1.setVegetableName("Radish [1/1]");
        calcBean1.setVegetablePath("/res/radish.png");
        add(calcBean1, new java.awt.GridBagConstraints());
    }// </editor-fold>//GEN-END:initComponents


    // Variables declaration - do not modify//GEN-BEGIN:variables
    private ui.beans.VegBean calcBean1;
    // End of variables declaration//GEN-END:variables
}
