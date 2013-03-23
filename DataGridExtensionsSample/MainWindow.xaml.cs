﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DataGridExtensions;

namespace DataGridExtensionsSample
{
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random rand = new Random();

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Provide a simple list of 100 random items.
        /// </summary>
        public IEnumerable<DataItem> Items
        {
            get
            {
                return Enumerable.Range(0, 100).Select(index => new DataItem(rand, index)).ToArray();
            }
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            // Use the integer filter for the integer column.
            if (e.PropertyType == typeof(int))
            {
                e.Column.SetTemplate((ControlTemplate)this.FindResource("IntegerFilter"));
            }
            else if ((e.PropertyType != typeof(bool)) && e.PropertyType.IsPrimitive)
            {
                // Hide the filter for all other primitive data types except bool.
                // Here we will hide the filter for the double DataItem.Probability.
                e.Column.SetIsFilterVisible(false);
            }
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            // must defer action, filters not yet available in OnLoaded.
            Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(() =>
            {
                // Sample to manipulate the filter values by code.
                var column = grid1.Columns[0];

                var control = grid1.GetFilter().FilterColumnControls.FirstOrDefault(c => c.Column.Equals(column));
                if (control != null)
                {
                    control.Filter = "True";
                }
            }));
        }
    }
}