using System;
using System.Windows;
using System.Windows.Controls;

namespace mEQUIPoctet.Source.UI
{
    [Flags]
    enum Proctype : int
    {
        None = 0,
        NoDeathDrop = 1,
        NoDrop = 2,
        NoSell = 4,
        CashItem = 8,
        NoTrade = 16,
        CanBind = 64,
        LeaveRemove = 256,
        PickupUse = 512,
        DeathDrop = 1024,
        LogoffRemove = 2048,
        NoRepair = 4096,
        NoAccountStash = 16384,
        BoundCosmetic = 32768
    }

    /// <summary>
    /// Proctype calculator.
    /// </summary>
    public partial class ProctypeWindow : Window
    {
        /// <summary>
        /// Whether the components are marked as locked.
        /// </summary>
        /// <remarks>
        /// Prevents the TextChanged handler from triggering when modifying checkboxes, and vice versa.
        /// </remarks>
        bool isLocked = false;

        public ProctypeWindow()
        {
            InitializeComponent();
        }

        private void CalculateProctype(object sender, RoutedEventArgs e)
        {
            if (!(ProctypeTextBox is TextBox))
            {
                return;
            }

            try
            {
                if (isLocked)
                {
                    return;
                }

                isLocked = true;

                Proctype proctype = Proctype.None;

                proctype |= (NoDeathDropCheckBox?.IsChecked ?? false) ? Proctype.NoDeathDrop : Proctype.None;
                proctype |= (NoDropCheckBox?.IsChecked ?? false) ? Proctype.NoDrop : Proctype.None;
                proctype |= (NoSellCheckBox?.IsChecked ?? false) ? Proctype.NoSell : Proctype.None;
                proctype |= (CashItemCheckBox?.IsChecked ?? false) ? Proctype.CashItem : Proctype.None;
                proctype |= (NoTradeCheckBox?.IsChecked ?? false) ? Proctype.NoTrade : Proctype.None;
                proctype |= (CanBindCheckBox?.IsChecked ?? false) ? Proctype.CanBind : Proctype.None;
                proctype |= (LeaveRemoveCheckBox?.IsChecked ?? false) ? Proctype.LeaveRemove : Proctype.None;
                proctype |= (PickupUseCheckBox?.IsChecked ?? false) ? Proctype.PickupUse : Proctype.None;
                proctype |= (DeathDropCheckBox?.IsChecked ?? false) ? Proctype.DeathDrop : Proctype.None;
                proctype |= (LogoffRemoveCheckBox?.IsChecked ?? false) ? Proctype.LogoffRemove : Proctype.None;
                proctype |= (NoRepairCheckBox?.IsChecked ?? false) ? Proctype.NoRepair : Proctype.None;
                proctype |= (NoAccountStashCheckBox?.IsChecked ?? false) ? Proctype.NoAccountStash : Proctype.None;
                proctype |= (BoundCosmeticCheckBox?.IsChecked ?? false) ? Proctype.BoundCosmetic : Proctype.None;

                ProctypeTextBox.Text = ((int)proctype).ToString();

                isLocked = false;
            }
            catch
            {
                isLocked = false;
                throw;
            }
        }

        private void ProctypeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Ensure all components are initialized.
            if (!(NoDeathDropCheckBox is CheckBox) ||
                !(NoDropCheckBox is CheckBox) ||
                !(NoSellCheckBox is CheckBox) ||
                !(CashItemCheckBox is CheckBox) ||
                !(NoTradeCheckBox is CheckBox) ||
                !(CanBindCheckBox is CheckBox) ||
                !(LeaveRemoveCheckBox is CheckBox) ||
                !(PickupUseCheckBox is CheckBox) ||
                !(DeathDropCheckBox is CheckBox) ||
                !(LogoffRemoveCheckBox is CheckBox) ||
                !(NoRepairCheckBox is CheckBox) ||
                !(NoAccountStashCheckBox is CheckBox) ||
                !(BoundCosmeticCheckBox is CheckBox))
            {
                return;
            }

            try
            {
                if (isLocked)
                {
                    return;
                }

                isLocked = true;

                if (int.TryParse(ProctypeTextBox?.Text, out int result))
                {
                    Proctype proctype = (Proctype)result;

                    NoDeathDropCheckBox.IsChecked = (proctype & Proctype.NoDeathDrop) == Proctype.NoDeathDrop;
                    NoDropCheckBox.IsChecked = (proctype & Proctype.NoDrop) == Proctype.NoDrop;
                    NoSellCheckBox.IsChecked = (proctype & Proctype.NoSell) == Proctype.NoSell;
                    CashItemCheckBox.IsChecked = (proctype & Proctype.CashItem) == Proctype.CashItem;
                    NoTradeCheckBox.IsChecked = (proctype & Proctype.NoTrade) == Proctype.NoTrade;
                    CanBindCheckBox.IsChecked = (proctype & Proctype.CanBind) == Proctype.CanBind;
                    LeaveRemoveCheckBox.IsChecked = (proctype & Proctype.LeaveRemove) == Proctype.LeaveRemove;
                    PickupUseCheckBox.IsChecked = (proctype & Proctype.PickupUse) == Proctype.PickupUse;
                    DeathDropCheckBox.IsChecked = (proctype & Proctype.DeathDrop) == Proctype.DeathDrop;
                    LogoffRemoveCheckBox.IsChecked = (proctype & Proctype.LogoffRemove) == Proctype.LogoffRemove;
                    NoRepairCheckBox.IsChecked = (proctype & Proctype.NoRepair) == Proctype.NoRepair;
                    NoAccountStashCheckBox.IsChecked = (proctype & Proctype.NoAccountStash) == Proctype.NoAccountStash;
                    BoundCosmeticCheckBox.IsChecked = (proctype & Proctype.BoundCosmetic) == Proctype.BoundCosmetic;
                }

                isLocked = false;
            }
            catch
            {
                isLocked = false;
                throw;
            }
        }
    }
}
