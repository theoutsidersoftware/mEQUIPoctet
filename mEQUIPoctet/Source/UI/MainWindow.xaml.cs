using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using mEQUIPoctet.Source.Config;
using mEQUIPoctet.Source.Core;

namespace mEQUIPoctet.Source.UI
{
    /// <inheritdoc cref="Window" />
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string _octet = "";

        /// <summary>
        /// The raw octet string representing the equipment.
        /// </summary>
        public string Octet
        {
            get
            {
                return _octet;
            }

            set
            {
                _octet = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// The equipment represented by the octet string.
        /// </summary>
        public EquipmentViewModel Equipment { get; set; } = new EquipmentViewModel();

        // SelectionChanged events are triggered even when making programmatic modifications. Locks are needed to
        // prevent conflicts.
        private readonly ISet<string> _componentLocks = new HashSet<string>();

        public MainWindow()
        {
            try
            {
                Presets.Load(@"presets.ini");
            }
            catch (Exception)
            {
                MessageBox.Show(@"Configuration file ""presets.ini"" not found in working directory." +
                                @"This application will run with degraded functionalities.",
                                @"Unable to Load presets.ini",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }

            InitializeComponent();
            SynchronizeComponent();
        }

        /// <summary>
        /// Mark the component with the given identifer as locked, so that other event handlers should not use it.
        /// </summary>
        /// <param name="componentName">The component identifer.</param>
        /// <returns>Whether the lock was successful.</returns>
        private bool LockComponent(string componentName)
        {
            return _componentLocks.Add(componentName);
        }

        /// <summary>
        /// Unmarks the component with the given identifer, allowing other event handlers to use it.
        /// </summary>
        /// <param name="componentName">The component identifer.</param>
        /// <returns>Whether the unlock was successful.</returns>
        private bool UnlockComponent(string componentName)
        {
            return _componentLocks.Remove(componentName);
        }

        /// <summary>
        /// Enforces the TextBox to contain an integer.
        /// </summary>
        private void TextBoxEnforceInteger(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox == null)
            {
                return;
            }

            try
            {
                textBox.Text = int.Parse(textBox.Text, NumberStyles.Integer | NumberStyles.AllowThousands).ToString();
            }
            catch (OverflowException)
            {
                if (textBox.Text.Trim().StartsWith("-"))
                {
                    textBox.Text = int.MinValue.ToString();
                }

                textBox.Text = int.MaxValue.ToString();
            }
            catch
            {
                textBox.Text = "0";
            }
        }

        /// <summary>
        /// Parses the Octet string into an Equipment representation.
        /// </summary>
        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OctetParser parser = new OctetParser(Octet);
                Equipment.Import(parser.Parse());
                SynchronizeComponent();
            }
            catch
            {
                MessageBox.Show(@"Octet is invalid.",
                                @"Import Failed",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Generate an Octet string that represents the Equipment.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            OctetSerializer serializer = new OctetSerializer(Equipment.Export());
            Octet = serializer.Serialize();
            OctetTextBox.Focus();
            OctetTextBox.SelectAll();
        }

        private void MaskMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Window window = new MaskWindow();
            window.Show();
        }

        private void ProctypeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Window window = new ProctypeWindow();
            window.Show();
        }
        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Window window = new AboutWindow();
            window.Show();
        }


        /// <summary>
        /// Synchronzies all component with the current Equipment.
        /// </summary>
        private void SynchronizeComponent()
        {
            SynchronizeClassesListBox(Equipment.Classes);
            SynchronizeSignatureComponent(Equipment.SignatureType);
        }

        /// <summary>
        /// Event handler for manually inputting WeaponMajorType.
        /// </summary>
        /// <remarks>This allows the combo box to be updated as text is being entered.</remarks>
        private void WeaponMajorTypeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(WeaponMajorTypeTextBox?.Text, out int result) && result != 0)
            {
                WeaponMajorTypeTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
        }

        /// <summary>
        /// Event handler for manually inputting projectile.
        /// </summary>
        /// <remarks>This allows the combo box to be updated as text is being entered.</remarks>
        private void ProjectileTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(ProjectileTextBox?.Text, out int result) && result != 0)
            {
                ProjectileTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
        }

        /// <summary>
        /// Event handler for manually inputting GFX.
        /// </summary>
        /// <remarks>This allows the combo box to be updated as text is being entered.</remarks>
        private void GFXTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (short.TryParse(GFXTextBox?.Text, out short result) && result != 0)
            {
                GFXTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
        }

        /// <summary>
        /// Event handler for manually inputting refine id.
        /// </summary>
        /// <remarks>This allows the combo box to be updated as text is being entered.</remarks>
        private void RefineIdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(RefineIdTextBox?.Text, out int result) && result != 0)
            {
                RefineIdTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
        }

        /// <summary>
        /// Event handler for manually selecting classes.
        /// </summary>
        private void ClassesTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SynchronizeClassesListBox(ClassesTextBox?.Text);
        }

        /// <summary>
        /// Synchronizes the ClassesListBox with the given value of classes.
        /// </summary>
        /// <param name="classes">The value of classes to synchronize to.</param>
        private void SynchronizeClassesListBox(string classes)
        {
            // Ensure all needed components are initialized.
            if (!(ClassesListBox is ListBox))
            {
                return;
            }

            try
            {
                if (!LockComponent("Classes"))
                {
                    return;
                }

                ClassesListBox.UnselectAll();

                // ClassesListBox ItemsSource is Presets.Classes.
                foreach (KeyValuePair<string, string> item in ClassesListBox.Items)
                {
                    if (short.TryParse(item.Key, out short parsedKey) &&
                        short.TryParse(classes, out short parsedClasses))
                    {
                        if ((parsedClasses & parsedKey) == parsedKey)
                        {
                            ClassesListBox.SelectedItems.Add(item);
                        }
                    }
                }

                UnlockComponent("Classes");
            }
            catch
            {
                UnlockComponent("Classes");
                throw;
            }
        }

        /// <summary>
        /// Event handler for manually selecting classes.
        /// </summary>
        private void ClassesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Ensure all needed components are initialized.
            if (!(ClassesListBox is ListBox) ||
                !(ClassesTextBox is TextBox) ||
                ClassesListBox?.SelectedItems == null)
            {
                return;
            }

            try
            {
                if (!LockComponent("Classes"))
                {
                    return;
                }

                short classes = 0;

                // ClassesListBox ItemsSource is Presets.Classes.
                foreach (KeyValuePair<string, string> selectedItem in ClassesListBox.SelectedItems)
                {
                    if (short.TryParse(selectedItem.Key, out short itemKey))
                    {
                        classes += itemKey;
                    }
                }

                Equipment.Classes = classes.ToString();

                UnlockComponent("Classes");
            }
            catch
            {
                UnlockComponent("Classes");
                throw;
            }
        }

        /// <summary>
        /// Handler for manually selecting SignatureType.
        /// </summary>
        private void SignatureTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SignatureTypeComboBox?.SelectedValue is byte)
            {
                SynchronizeSignatureComponent((byte)SignatureTypeComboBox.SelectedValue);
                return;
            }

            if (byte.TryParse(SignatureTypeComboBox?.SelectedValue?.ToString(), out byte signatureType))
            {
                SynchronizeSignatureComponent(signatureType);
            }
        }

        /// <summary>
        /// Synchronizes components related to Signatures with the given value of signature type.
        /// </summary>
        /// <param name="signatureType">The value of signature type to synchronize to.</param>
        private void SynchronizeSignatureComponent(byte signatureType)
        {
            // Ensure all needed components are initialized.
            if (!(SignatureTextBox is TextBox) ||
                !(InkColorButton is Button))
            {
                return;
            }

            if (signatureType == 4)
            {
                SignatureTextBox.IsEnabled = true;
                InkColorButton.IsEnabled = false;
                // Reset color to black.
                SetSignatureTextBoxColor(0);
            }
            else if (signatureType == 5)
            {
                SignatureTextBox.IsEnabled = true;
                InkColorButton.IsEnabled = true;
                SetSignatureTextBoxColor(Equipment.InkColor);
            }
            else
            {
                SignatureTextBox.IsEnabled = false;
                InkColorButton.IsEnabled = false;
                // Reset color to black.
                SetSignatureTextBoxColor(0);
            }
        }

        /// <summary>
        /// Sets components related to Signature with the given color.
        /// </summary>
        /// <param name="color">The color of the signature.</param>
        /// <remarks>Do not call directly. Call SynchronizeSignatureComponent instead.</remarks>
        /// <see cref="SynchronizeSignatureComponent"/>
        private void SetSignatureTextBoxColor(short color)
        {
            Color15 inkColor = new Color15(color);

            var previewColor = inkColor.ConvertTo24();

            var previewBrush = new System.Windows.Media.SolidColorBrush(previewColor);

            SignatureTextBox.Foreground = previewBrush;

            // Invert the background color if the text becomes difficult to see.
            // Based on https://stackoverflow.com/questions/11867545/change-text-color-based-on-brightness-of-the-covered-background-area
            int brightness = (int)Math.Round(((previewColor.R * 299) +
                                              (previewColor.G * 587) +
                                              (previewColor.B * 114)) / 1000.0f);

            if (brightness > 125)
            {
                SignatureTextBox.Background = System.Windows.Media.Brushes.Black;
            }
            else
            {
                SignatureTextBox.Background = System.Windows.Media.Brushes.White;
            }
        }

        /// <summary>
        /// Handler for manually selecting the Ink Color.
        /// </summary>
        private void InkColorButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();

            colorDialog.AllowFullOpen = true;
            colorDialog.AnyColor = true;
            colorDialog.FullOpen = true;

            if (colorDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            // Convert from WinForms to WPF.
            System.Drawing.Color color = colorDialog.Color;
            var color24 = System.Windows.Media.Color.FromRgb(color.R, color.G, color.B);

            // Convert to 15-bit.
            Color15 color15 = Color15.ConvertFrom24(color24);

            Equipment.InkColor = color15.Color;

            SynchronizeSignatureComponent(Equipment.SignatureType);
        }

        /// <summary>
        /// Event handler for manually selecting a preset soulgem.
        /// </summary>
        private void SoulgemComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetPresetSoulgem();
        }

        /// <summary>
        /// Event handler for manually selecting a preset soulgem.
        /// </summary>
        private void SoulgemComboBox_DropDownClosed(object sender, EventArgs e)
        {
            SetPresetSoulgem();
        }

        /// <summary>
        /// Set all components related to soulgems to the selected preset soulgem.
        /// </summary>
        private void SetPresetSoulgem()
        {
            // Ensure all needed components are initialized.
            if (!(SoulgemTextBox is TextBox) ||
                !(SoulgemAddonCheckBox is CheckBox) ||
                !(SoulgemComboBox is ComboBox))
            {
                return;
            }

            try
            {
                if (!LockComponent("Soulgem"))
                {
                    return;
                }

                SoulgemAddonCheckBox.IsEnabled = false;
                SoulgemAddonCheckBox.IsChecked = false;

                // SoulgemComboBox ItemSource is Presets.Soulgem.
                var wrapper = SoulgemComboBox.SelectedItem as KeyValuePair<string, string[]>?;

                if (!wrapper.HasValue)
                {
                    return;
                }

                KeyValuePair<string, string[]> selectedItem = wrapper.Value;
                string soulgemId = selectedItem.Key;
                string[] soulgemPreset = selectedItem.Value;

                if (string.IsNullOrWhiteSpace(soulgemId))
                {
                    return;
                }

                SoulgemTextBox.Text = soulgemId;

                if ((Equipment.Type == EquipmentType.Weapon && soulgemPreset.Length > 1) ||
                    (Equipment.Type == EquipmentType.Armor && soulgemPreset.Length > 2) ||
                    (Equipment.Type == EquipmentType.Accessory && soulgemPreset.Length > 3))
                {
                    SoulgemAddonCheckBox.IsEnabled = true;
                    SoulgemAddonCheckBox.IsChecked = true;
                }
                else if (soulgemPreset.Length <= 1)
                {
                    SoulgemAddonCheckBox.IsEnabled = false;
                    SoulgemAddonCheckBox.IsChecked = false;
                }
                else
                {
                    SoulgemAddonCheckBox.IsEnabled = true;
                    SoulgemAddonCheckBox.IsChecked = false;
                }

                UnlockComponent("Soulgem");
            }
            catch
            {
                UnlockComponent("Soulgem");
                throw;
            }
        }

        /// <summary>
        /// Handler to load the information of the selected socket.
        /// </summary>
        private void SocketsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Ensure all needed components are initialized.
            if (!(SoulgemTextBox is TextBox) ||
                !(SoulgemAddonCheckBox is CheckBox) ||
                !(SocketsListView is SelectableListView))
            {
                return;
            }

            Socket socket = SocketsListView.SelectedItem as Socket;

            if (socket == null)
            {
                return;
            }

            SoulgemTextBox.Text = socket.Soulgem.ToString();
            SoulgemAddonCheckBox.IsChecked = socket.HasAddon;
        }

        /// <summary>
        /// Handler to add a new socket.
        /// </summary>
        private void SocketAddButton_Click(object sender, RoutedEventArgs e)
        {
            // Ensure all needed components are initialized.
            if (!(SoulgemTextBox is TextBox) ||
                !(SoulgemAddonCheckBox is CheckBox) ||
                !(SocketsListView is SelectableListView))
            {
                return;
            }

            Socket socket = new Socket();
            socket.SetSoulgem(SoulgemTextBox.Text);
            socket.HasAddon = SoulgemAddonCheckBox.IsChecked ?? false;

            Equipment.Sockets.Add(socket);
            SocketsListView.Items.Refresh();
        }

        /// <summary>
        /// Handler to remove a selected socket.
        /// </summary>
        private void SocketRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            Socket socket = SocketsListView.SelectedItem as Socket;

            if (socket == null)
            {
                return;
            }

            Equipment.Sockets.Remove(socket);
            SocketsListView.Items.Refresh();
        }

        /// <summary>
        /// Synchronizes components related to soulgem with the given value of soulgem id.
        /// </summary>
        /// <param name="soulgem">The value of the soulgem's id to synchronize to.</param>
        private void SoulgemTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Ensure all needed components are initialized.
            if (!(SoulgemAddonCheckBox is CheckBox) ||
                !(SoulgemComboBox is ComboBox))
            {
                return;
            }

            try
            {
                if (!LockComponent("Soulgem"))
                {
                    return;
                }

                if (!int.TryParse(SoulgemTextBox?.Text, out int soulgem))
                {
                    return;
                }

                SoulgemAddonCheckBox.IsEnabled = false;
                SoulgemAddonCheckBox.IsChecked = false;

                SoulgemComboBox.SelectedValue = soulgem;

                if (!Presets.Soulgem.ContainsKey(soulgem.ToString()))
                {
                    return;
                }

                string[] soulgemPreset = Presets.Soulgem[soulgem.ToString()];

                if ((Equipment.Type == EquipmentType.Weapon && soulgemPreset.Length > 1) ||
                    (Equipment.Type == EquipmentType.Armor && soulgemPreset.Length > 2) ||
                    (Equipment.Type == EquipmentType.Accessory && soulgemPreset.Length > 3))
                {
                    SoulgemAddonCheckBox.IsEnabled = true;
                    SoulgemAddonCheckBox.IsChecked = true;
                }
                else if (soulgemPreset.Length <= 1)
                {
                    SoulgemAddonCheckBox.IsEnabled = false;
                    SoulgemAddonCheckBox.IsChecked = false;
                }
                else
                {
                    SoulgemAddonCheckBox.IsEnabled = true;
                    SoulgemAddonCheckBox.IsChecked = false;
                }

                UnlockComponent("Soulgem");
            }
            catch
            {
                UnlockComponent("Soulgem");
                throw;
            }
        }

        /// <summary>
        /// Handler to load the information of the selected addon.
        /// </summary>
        private void AddonsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Ensure all needed components are initialized.
            if (!(AddonIdTextBox is TextBox) ||
                !(AddonValueTextBox is TextBox) ||
                !(AddonParam2TextBox is TextBox) ||
                !(AddonParam3TextBox is TextBox) ||
                !(AddonTypeComboBox is ComboBox) ||
                !(AddonHiddenCheckBox is CheckBox))
            {
                return;
            }

            Addon addon = AddonsListView?.SelectedItem as Addon;

            if (addon == null)
            {
                return;
            }

            AddonIdTextBox.Text = addon.Id.ToString();
            AddonValueTextBox.Text = addon.Value.ToString();
            AddonParam2TextBox.Text = addon.Param2.ToString();
            AddonParam3TextBox.Text = addon.Param3.ToString();
            AddonHiddenCheckBox.IsChecked = addon.Hidden;
            AddonTypeComboBox.SelectedValue = addon.Type;
        }

        /// <summary>
        /// Event handler for manually selecting a preset addon.
        /// </summary>
        private void AddonComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetPresetAddon();
        }

        /// <summary>
        /// Event handler for manually selecting a preset addon.
        /// </summary>
        private void AddonComboBox_DropDownClosed(object sender, EventArgs e)
        {
            SetPresetAddon();
        }

        /// <summary>
        /// Set all components related to addons to the selected preset addon.
        /// </summary>
        private void SetPresetAddon()
        {
            // Ensure all needed components are initialized.
            if (!(AddonComboBox is ComboBox) ||
                !(AddonIdTextBox is TextBox) ||
                !(AddonValueTextBox is TextBox) ||
                !(AddonParam2TextBox is TextBox) ||
                !(AddonParam3TextBox is TextBox) ||
                !(AddonTypeComboBox is ComboBox) ||
                !(AddonHiddenCheckBox is CheckBox))
            {
                return;
            }

            try
            {
                if (!LockComponent("Addon"))
                {
                    return;
                }

                // AddonComboBox ItemSource is Presets.Addon.
                var wrapper = AddonComboBox?.SelectedItem as KeyValuePair<string, string[]>?;

                if (!wrapper.HasValue)
                {
                    return;
                }

                KeyValuePair<string, string[]> addon = wrapper.Value;

                if (string.IsNullOrWhiteSpace(addon.Key))
                {
                    return;
                }

                AddonIdTextBox.Text = addon.Key;
                AddonValueTextBox.Text = "0";
                AddonParam2TextBox.Text = "0";
                AddonParam3TextBox.Text = "0";
                

                // All preset addons are assumed not hidden, since hidden addons are meant for soulgems.
                AddonHiddenCheckBox.IsChecked = false;

                if (addon.Value.Length > 0)
                {
                    AddonTypeComboBox.SelectedValue = AddonType.Unidentified;
                }

                if (addon.Value.Length > 1)
                {
                    AddonTypeComboBox.SelectedValue = AddonType.Normal;
                    AddonValueTextBox.Text = addon.Value[1];
                }

                if (addon.Value.Length > 2)
                {
                    AddonTypeComboBox.SelectedValue = AddonType.UniqueOffensive;
                    AddonParam2TextBox.Text = addon.Value[2];
                }

                if (addon.Value.Length > 3)
                {
                    AddonTypeComboBox.SelectedValue = AddonType.UniqueDefensive;
                    AddonParam3TextBox.Text = addon.Value[3];
                }

                UnlockComponent("Addon");
            }
            catch
            {
                UnlockComponent("Addon");
                throw;
            }
        }

        private void AddonTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Ensure all needed components are initialized.
            if (!(AddonTypeComboBox is ComboBox) ||
                !(AddonParam2TextBox is TextBox) ||
                !(AddonParam3TextBox is TextBox))
            {
                return;
            }

            // AddonTypeComboBox ItemSource is Presets.AddonTypes.
            var wrapper = AddonTypeComboBox?.SelectedItem as KeyValuePair<AddonType, string>?;

            if (!wrapper.HasValue)
            {
                return;
            }

            KeyValuePair<AddonType, string> selectedItem = wrapper.Value;

            if (selectedItem.Key == AddonType.Normal)
            {
                AddonValueTextBox.IsEnabled = true;
                AddonParam2TextBox.IsEnabled = false;
                AddonParam3TextBox.IsEnabled = false;
            }
            else if (selectedItem.Key == AddonType.UniqueOffensive)
            {
                AddonValueTextBox.IsEnabled = true;
                AddonParam2TextBox.IsEnabled = true;
                AddonParam3TextBox.IsEnabled = false;
            }
            else if (selectedItem.Key == AddonType.UniqueDefensive)
            {
                AddonValueTextBox.IsEnabled = true;
                AddonParam2TextBox.IsEnabled = true;
                AddonParam3TextBox.IsEnabled = true;
            }
            else
            {
                AddonValueTextBox.IsEnabled = false;
                AddonParam2TextBox.IsEnabled = false;
                AddonParam3TextBox.IsEnabled = false;
            }
        }

        /// <summary>
        /// Event handler for manually inputting addon.
        /// </summary>
        private void AddonIdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(AddonComboBox is ComboBox))
            {
                return;
            }

            try
            {
                if (!LockComponent("Addon"))
                {
                    return;
                }

                if (int.TryParse(AddonIdTextBox?.Text, out int addonId))
                {
                    AddonComboBox.SelectedValue = addonId;
                }

                UnlockComponent("Addon");
            }
            catch
            {
                UnlockComponent("Addon");
                throw;
            }            
        }

        private void AddonAddButton_Click(object sender, RoutedEventArgs e)
        {
            Addon addon = new Addon();

            addon.Hidden = AddonHiddenCheckBox.IsChecked ?? false;

            if (!int.TryParse(AddonIdTextBox?.Text, out int addonId) || addonId == 0)
            {
                MessageBox.Show(@"Addon must have non-zero Id.",
                                @"Cannot add Addon",
                                MessageBoxButton.OK,
                                MessageBoxImage.None);
                return;
            }

            addon.Id = addonId;

            var wrapper = AddonTypeComboBox?.SelectedItem as KeyValuePair<AddonType, string>?;

            if (!wrapper.HasValue)
            {
                MessageBox.Show(@"Addon must have Type.",
                                @"Cannot add Addon",
                                MessageBoxButton.OK,
                                MessageBoxImage.None);
                return;
            }

            KeyValuePair<AddonType, string> selectedItem = wrapper.Value;

            addon.Type = selectedItem.Key;

            decimal addonValue;
            int addonParam2;
            int addonParam3;
            if (selectedItem.Key == AddonType.Normal &&
                decimal.TryParse(AddonValueTextBox.Text, out addonValue))
            {
                addon.Value = addonValue;
            }
            else if (selectedItem.Key == AddonType.UniqueOffensive &&
                decimal.TryParse(AddonValueTextBox.Text, out addonValue) &&
                int.TryParse(AddonParam2TextBox.Text, out addonParam2))
            {
                addon.Value = addonValue;
                addon.Param2 = addonParam2;
            }
            else if (selectedItem.Key == AddonType.UniqueDefensive &&
                     decimal.TryParse(AddonValueTextBox.Text, out addonValue) &&
                     int.TryParse(AddonParam2TextBox.Text, out addonParam2) &&
                     int.TryParse(AddonParam3TextBox.Text, out addonParam3))
            {
                addon.Value = addonValue;
                addon.Param2 = addonParam2;
                addon.Param3 = addonParam3;
            }
            else
            {
                return;
            }

            Equipment.Addons.Add(addon);
            AddonsListView.Items.Refresh();
            AddonsListView.ScrollIntoView(addon);
        }

        private void AddonRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in AddonsListView.SelectedItems)
            {
                Addon addon = item as Addon;

                if (addon == null)
                {
                    continue;
                }

                Equipment.Addons.Remove(addon);
            }
            
            AddonsListView.Items.Refresh();
        }
    }
}
