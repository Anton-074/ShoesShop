using Microsoft.EntityFrameworkCore;
using ShoesShop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ShoesShop
{
    public partial class FormOrders : Form
    {
        public User CurrentUser { get; private set; }
        public FormOrders(User user)
        {
            InitializeComponent();
            CurrentUser = user;
            LoadOrders();
        }
        private void LoadOrders()
        {
            try
            {
                using (var db = new ShopDbContext())
                {
                    
                    this.dgvOrders.DataSource = db.Orders
                        .Include(i => i.User)
                        .Include(i => i.DeliveryPoint)
                        .Include(i => i.Status)
                        .Where(w=>w.User.Id == CurrentUser.Id)
                        .Select(g => new
                        {
                            g.Id,
                            g.User.FullName,
                            g.OrderDate,
                            g.DeliveryDate,
                            g.DeliveryPoint.DeliveryAddress,
                            g.Code,
                            g.Status.StatusName,
                        })
                        .ToList();

                        dgvOrders.Columns["Id"].Visible = false;
                        dgvOrders.Columns["FullName"].HeaderText = "ФИО";
                        dgvOrders.Columns["OrderDate"].HeaderText = "Дата заказа";
                        dgvOrders.Columns["DeliveryDate"].HeaderText = "Дата доставки";
                        dgvOrders.Columns["StatusName"].HeaderText = "Статус";
                        dgvOrders.Columns["DeliveryAddress"].HeaderText = "Адрес";
                        dgvOrders.Columns["Code"].HeaderText = "Код";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
