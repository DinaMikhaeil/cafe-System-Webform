﻿using Cafffe_Sytem.CustomModels;
using Cafffe_Sytem.Pages.SacondryPages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cafffe_Sytem.Pages
{
    public partial class Offers:Templete
    {
      

        public Offers()
        {
            InitializeComponent();
           
            PopulateComboBoxphone();
            PopulateComboBoxofferend();
            init();
        }


        private void search_btn_Click(object sender, EventArgs e)
        {
            try { 
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = Color.White; // Set background color to default
                }
            }

            // Get the search keyword from the search box
            string keyword = searchvalue_txt.Text.Trim().ToLower();
            int searchid=-1;
            // Ensure the keyword is not empty
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                bool matchFound = false;
               
                // Iterate through each row in the DataGridView
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    // Iterate through each cell in the current row
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        // Check if the cell value contains the keyword
                        if (cell.Value != null && cell.Value.ToString().ToLower().Contains(keyword))
                        {
                            searchid = (int)row.Cells["ID"].Value; 
                            // Highlight the cell
                            cell.Style.BackColor = Color.Yellow;
                            matchFound = true; // Set flag to indicate at least one match found
                        }
                    }
                }

                if (!matchFound)
                {
                    // If no matches were found, show a message to the user
                    MessageBox.Show("No matching records found.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    dataGridView1.DataSource=null;
                    dataGridView1.DataSource=DBConnection.Context.Offers.Where(c=>c.Off_ID== searchid).Select(c => new { ID = c.Off_ID, Name = c.Off_Name, Limit = c.Off_Limit, c.Off_Start, c.Off_End }).ToList();

                }
            }
            else
            {
                // Display a message to the user if the search keyword is empty
                MessageBox.Show("Please enter a search keyword.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = DBConnection.Context.Offers.Select(c => new { ID = c.Off_ID, Name = c.Off_Name, Limit = c.Off_Limit, c.Off_Start, c.Off_End }).ToList();
            }
            }
            catch (Exception ex)
            {

                DialogResult result = MessageBox.Show("System Error : " + ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        public void refresh(object obj,EventArgs e)
        {
            init();
        }
        private void update_btn_Click(object sender, EventArgs e)
        {
            try { 
            MangeOffer mangeOffer = new MangeOffer("update");

            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the data from the selected row
                var selectedRow = dataGridView1.SelectedRows[0];
                var offerOff_ID = (int)selectedRow.Cells["ID"].Value;
                var offerOff_Name = selectedRow.Cells["Name"].Value.ToString();
                int offerOff_Limit =int.Parse( selectedRow.Cells["Limit"].Value.ToString());
                DateTime offerOff_Start = Convert.ToDateTime(selectedRow.Cells["Off_Start"].Value.ToString());
                DateTime offerOff_End = Convert.ToDateTime(selectedRow.Cells["Off_End"].Value.ToString());

                // Create an instance of the MangeClients form

                // Pass the data to the MangeClients form for updating
                mangeOffer.InitializeForUpdate(offerOff_ID, offerOff_Name, offerOff_Limit, offerOff_Start, offerOff_End);

                // Show the MangeClients form
                mangeOffer.Show();
                mangeOffer.eva += this.refresh;

                init();
            }
            else
            {
                MessageBox.Show("Please select a row to update.");
            }
            }
            catch (Exception ex)
            {

                DialogResult result = MessageBox.Show("System Error : " + ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private void add_btn_Click(object sender, EventArgs e)
        {
            try { 
            MangeOffer mangeOffer = new MangeOffer("Add");
            mangeOffer.Show();
            mangeOffer.eva += this.refresh;
            }
            catch (Exception ex)
            {

                DialogResult result = MessageBox.Show("System Error : " + ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void delet_btn_Click(object sender, EventArgs e)

        {
            try { // Show the message box asking the user to confirm deletion
            DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Confirmation", MessageBoxButtons.OKCancel);

            // Check if the user clicked OK to confirm deletion
            if (result == DialogResult.OK)
            {
                // User confirmed deletion
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    // Get the ID of the selected Clint
                    int selectedofferID = (int)dataGridView1.SelectedRows[0].Cells["ID"].Value;

                    // Retrieve the corresponding Clint entity from the database
                    var selectedoffer = DBConnection.Context.Offers.FirstOrDefault(c => c.Off_ID == selectedofferID);

                    if (selectedoffer != null)
                    {
                        // Remove the selected Clint entity from the dbContext.Clints collection
                        DBConnection.Context.Offers.Remove(selectedoffer);

                        // Save changes to the database
                        DBConnection.Context.SaveChanges();

                        // Refresh the DataGridView to reflect the changes
                        init();
                    }
                    else
                    {
                        MessageBox.Show("Selected Clint not found in the database.");
                    }
                }
                else
                {
                    MessageBox.Show("Please select a row to delete.");
                }
            }
            }
            catch (Exception ex)
            {

                DialogResult result = MessageBox.Show("System Error : " + ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void init()
        {
            try { 
            var x = DBConnection.Context.Offers.Select(c => new { ID = c.Off_ID, Name = c.Off_Name, Limit = c.Off_Limit, c.Off_Start, c.Off_End }).ToList();
            dataGridView1.DataSource = x;
            PopulateComboBoxofferend();
            PopulateComboBoxphone();
            }
            catch (Exception ex)
            {

                DialogResult result = MessageBox.Show("System Error : " + ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private void offer_start_filter_comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            try
            {
                // Get the selected shift start date from ComboBox
                string selectedShiftStartStr = offer_start_filter_comboBox1.SelectedItem?.ToString();

                if (selectedShiftStartStr != "All offers")
                {
                    // Convert the selected shift start date from string to DateTime
                    if (DateTime.TryParse(selectedShiftStartStr, out DateTime selectedShiftStartDate))
                    {
                        // Filter offers based on the selected shift start date
                        var filteredOffers = DBConnection.Context.Offers
                            .Where(c => DbFunctions.TruncateTime(c.Off_Start) == selectedShiftStartDate.Date)
                            .Select(c => new { ID = c.Off_ID, Name = c.Off_Name, Limit = c.Off_Limit, c.Off_Start, c.Off_End })
                            .ToList();

                        // Update DataGridView with filtered results
                        dataGridView1.DataSource = filteredOffers;
                    }
                    else
                    {
                        MessageBox.Show("Invalid date selected.");
                    }
                }
                else
                {
                    // If "All offers" is selected, display all offers
                    init(); // Assuming init() is a method that displays all offers
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while filtering offers: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void offer_start_filter_comboBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string enteredText = offer_start_filter_comboBox1.Text.Trim();

                if (!string.IsNullOrEmpty(enteredText))
                {
                    // Convert the entered text to DateTime if possible
                    if (DateTime.TryParse(enteredText, out DateTime selectedDate))
                    {
                        // Filter offers based on the entered date
                        var filteredOffers = DBConnection.Context.Offers
                            .Where(c => DbFunctions.TruncateTime(c.Off_Start) == selectedDate)
                            .Select(c => new { ID = c.Off_ID, Name = c.Off_Name, Limit = c.Off_Limit, c.Off_Start, c.Off_End })
                            .ToList();

                        // Update DataGridView with filtered results
                        dataGridView1.DataSource = filteredOffers;
                    }
                    else
                    {
                        // Filter offers based on the entered string
                        var filteredOffers = DBConnection.Context.Offers
                            .Where(c => c.Off_Start.ToString().Contains(enteredText))
                            .Select(c => new { ID = c.Off_ID, Name = c.Off_Name, Limit = c.Off_Limit, c.Off_Start, c.Off_End })
                            .ToList();

                        // Update DataGridView with filtered results
                        dataGridView1.DataSource = filteredOffers;
                    }
                }
                else
                {
                    // If no text is entered, display all offers
                    offer_start_filter_comboBox1_SelectedIndexChanged(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while filtering offers: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void PopulateComboBoxphone()
        {
            try {
            offer_start_filter_comboBox1.Items.Clear(); // Clear existing items in the ComboBox

            // Add "All Users" option as the first item in the ComboBox
            offer_start_filter_comboBox1.Items.Add("All offers");

            // Get unique phone numbers from the Clients table
            var uniquePhoneNumbers = DBConnection.Context.Offers.Select(c => c.Off_Start).Distinct().ToList();

            // Add unique phone numbers to the ComboBox
            offer_start_filter_comboBox1.Items.AddRange(uniquePhoneNumbers.Select(p => p.ToString()).ToArray());
            }
            catch (Exception ex)
            {

                DialogResult result = MessageBox.Show("System Error : " + ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }



        private void Offer_end_filter_comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                // Get the selected shift start date from ComboBox
                string selectedShiftStartStr = Offer_end_filter_comboBox1.SelectedItem?.ToString();

                if (selectedShiftStartStr != "All offers")
                {
                    // Convert the selected shift start date from string to DateTime
                    if (DateTime.TryParse(selectedShiftStartStr, out DateTime selectedShiftStartDate))
                    {
                        // Filter offers based on the selected shift start date
                        var filteredOffers = DBConnection.Context.Offers
                            .Where(c => DbFunctions.TruncateTime(c.Off_End) == selectedShiftStartDate.Date)
                            .Select(c => new { ID = c.Off_ID, Name = c.Off_Name, Limit = c.Off_Limit, c.Off_Start, c.Off_End })
                            .ToList();

                        // Update DataGridView with filtered results
                        dataGridView1.DataSource = filteredOffers;
                    }
                    else
                    {
                        MessageBox.Show("Invalid date selected.");
                    }
                }
                else
                {
                    // If "All offers" is selected, display all offers
                    init(); // Assuming init() is a method that displays all offers
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while filtering offers: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Offer_end_filter_comboBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string enteredText = Offer_end_filter_comboBox1.Text.Trim();

                if (!string.IsNullOrEmpty(enteredText))
                {
                    // Convert the entered text to DateTime if possible
                    if (DateTime.TryParse(enteredText, out DateTime selectedDate))
                    {
                        // Filter offers based on the entered date
                        var filteredOffers = DBConnection.Context.Offers
                            .Where(c => DbFunctions.TruncateTime(c.Off_End) == selectedDate)
                            .Select(c => new { ID = c.Off_ID, Name = c.Off_Name, Limit = c.Off_Limit, c.Off_Start, c.Off_End })
                            .ToList();

                        // Update DataGridView with filtered results
                        dataGridView1.DataSource = filteredOffers;
                    }
                    else
                    {
                        // Filter offers based on the entered string
                        var filteredOffers = DBConnection.Context.Offers
                            .Where(c => c.Off_End.ToString().Contains(enteredText))
                            .Select(c => new { ID = c.Off_ID, Name = c.Off_Name, Limit = c.Off_Limit, c.Off_Start, c.Off_End })
                            .ToList();

                        // Update DataGridView with filtered results
                        dataGridView1.DataSource = filteredOffers;
                    }
                }
                else
                {
                    // If no text is entered, display all offers
                    Offer_end_filter_comboBox1_SelectedIndexChanged(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while filtering offers: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void PopulateComboBoxofferend()
        {
            Offer_end_filter_comboBox1.Items.Clear(); // Clear existing items in the ComboBox

            // Add "All Users" option as the first item in the ComboBox
            Offer_end_filter_comboBox1.Items.Add("All offers");

            // Get unique phone numbers from the Clients table
            var uniquePhoneNumbers = DBConnection.Context.Offers.Select(c => c.Off_End).Distinct().ToList();

            // Add unique phone numbers to the ComboBox
            Offer_end_filter_comboBox1.Items.AddRange(uniquePhoneNumbers.Select(p => p.ToString()).ToArray());
        }

        private void searchvalue_txt_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(searchvalue_txt.Text))
            {
                init();
            }
        }
    }
}
