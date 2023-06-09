package ui.beans;

import javax.swing.border.TitledBorder;

public class VegBean extends javax.swing.JPanel {

    private final TitledBorder border;
    
    private String vegetableName;
    private String vegetablePath;
    private int count;
    
    public VegBean() {
        initComponents();
        
        border = javax.swing.BorderFactory.createTitledBorder(null, vegetableName + ": " + count,
                         javax.swing.border.TitledBorder.DEFAULT_JUSTIFICATION, 
                         javax.swing.border.TitledBorder.DEFAULT_POSITION, 
                         new java.awt.Font("Segoe UI", 1, 18)); 
        
        setBorder(border);
    }

    public void setVegetableName(String vegetableName)
    {
        this.vegetableName = vegetableName;
        updateText();
    }
    
    public String getVegetableName()
    {
        return vegetableName;
    }
    
    public void setVegetablePath(String vegetablePath)
    {
        this.vegetablePath = vegetablePath;
        lblImage.setIcon(new javax.swing.ImageIcon(getClass().getResource(vegetablePath)));
    }
    
    public String getVegetablePath()
    {
        return vegetablePath;
    }

    public int getCount() 
    {
        return count;
    }
    
    /**
     * This method is called from within the constructor to initialize the form.
     * WARNING: Do NOT modify this code. The content of this method is always
     * regenerated by the Form Editor.
     */
    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {

        btnMinus = new javax.swing.JButton();
        lblImage = new javax.swing.JLabel();
        btnPlus = new javax.swing.JButton();

        setBorder(javax.swing.BorderFactory.createTitledBorder(null, "Potato", javax.swing.border.TitledBorder.DEFAULT_JUSTIFICATION, javax.swing.border.TitledBorder.DEFAULT_POSITION, new java.awt.Font("Segoe UI", 1, 18))); // NOI18N
        setLayout(new java.awt.BorderLayout());

        btnMinus.setText("-");
        btnMinus.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                btnMinusActionPerformed(evt);
            }
        });
        add(btnMinus, java.awt.BorderLayout.WEST);

        lblImage.setHorizontalAlignment(javax.swing.SwingConstants.CENTER);
        lblImage.setIcon(new javax.swing.ImageIcon(getClass().getResource("/res/potato.png"))); // NOI18N
        add(lblImage, java.awt.BorderLayout.CENTER);

        btnPlus.setText("+");
        btnPlus.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                btnPlusActionPerformed(evt);
            }
        });
        add(btnPlus, java.awt.BorderLayout.EAST);
    }// </editor-fold>//GEN-END:initComponents

    private void btnMinusActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_btnMinusActionPerformed
        if (count > 0)
        {
            count--;
        }
        
        updateText();
    }//GEN-LAST:event_btnMinusActionPerformed

    private void btnPlusActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_btnPlusActionPerformed
        count++;
        updateText();
    }//GEN-LAST:event_btnPlusActionPerformed

    private void updateText()
    {
        border.setTitle(vegetableName + ": " + String.valueOf(count));
        repaint();
    }
    
    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.JButton btnMinus;
    private javax.swing.JButton btnPlus;
    private javax.swing.JLabel lblImage;
    // End of variables declaration//GEN-END:variables
}
