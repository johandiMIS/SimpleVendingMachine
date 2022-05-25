using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace vendingMachine
{
    public partial class Form1 : Form
    {
        public class VendingMenu
        {
            public int     Id      { set; get; }
            public string  Name    { set; get; }
            public int     Price   { set; get; }
            public int     Stock   { set; get; }
            public int     Qty     { set; get; }
            public int     Total   { set; get; }

        }


        public class VendingMachine
        {
            public List<VendingMenu> menus = new List<VendingMenu>();
            public int TotalPrice;
            public void AddMenu(VendingMenu _vendingMenu)
            {
                this.menus.Add(_vendingMenu);
            }
            public void AddStock(string menuName, int StockQty)
            {
                foreach (VendingMenu _vendingMenu in this.menus)
                {
                    if (_vendingMenu.Name == menuName)
                    {
                        _vendingMenu.Stock+=StockQty;
                    }
                }
            }
            public void AddItem(string menuName)
            {
                foreach (VendingMenu _vendingMenu in this.menus)
                {
                    if (_vendingMenu.Name == menuName && _vendingMenu.Qty < _vendingMenu.Stock)
                    {
                        _vendingMenu.Qty++;
                    }
                }
            }

            public void SubtractItem(string menuName)
            {
                foreach (VendingMenu _vendingMenu in this.menus)
                {
                    if (_vendingMenu.Name == menuName && _vendingMenu.Qty > 0 )
                    {
                        _vendingMenu.Qty--;
                    }
                }
            }

            public int CalculateTotalPrice()
            {
                this.TotalPrice = 0;

                foreach (VendingMenu _vendingMenu in this.menus)
                {
                    _vendingMenu.Total = _vendingMenu.Price * _vendingMenu.Qty;
                    this.TotalPrice += _vendingMenu.Total;
                }

                return TotalPrice; ;
            }
        }

        VendingMachine vendingMachine = new VendingMachine();

        public Form1()
        {
            InitializeComponent();
            OnLoad();
            #region Menu Event
            btBiskuit.Click += BtBiskuit_Click;
            btChips.Click += BtChips_Click;
            btOreo.Click += BtOreo_Click;
            btTango.Click += BtTango_Click;
            btCokelat.Click += BtCokelat_Click;
            btMinBiskuit.Click += BtMinBiskuit_Click;
            btMinChips.Click += BtMinChips_Click;
            btMinOreo.Click += BtMinOreo_Click;
            btMinTango.Click += BtMinTango_Click;
            btMinCokelat.Click += BtMinCokelat_Click;
            #endregion
            #region payment event
            btAdd2k.Click += BtAdd2k_Click;
            btAdd5k.Click += BtAdd5k_Click;
            btAdd10k.Click += BtAdd10k_Click;
            btAdd20k.Click += BtAdd20k_Click;
            btAdd50k.Click += BtAdd50k_Click;

            btSub2k.Click += BtSub2k_Click;
            btSub5k.Click += BtSub5k_Click;
            btSub10k.Click += BtSub10k_Click;
            btSub20k.Click += BtSub20k_Click;
            btSub50k.Click += BtSub50k_Click;
            #endregion
            btPay.Click += BtPay_Click;
            tbStock.TextChanged += TbStock_TextChanged;
            btAddStock.Click += BtAddStock_Click;
        }

        private void BtPay_Click(object sender, EventArgs e)
        {
            int _total = Convert.ToInt32(lbTotal.Text);
            int _payment = Convert.ToInt32(lbPayment.Text);
            if ( _total > _payment )
            {
                MessageBox.Show($"Payment Not Enough");
                return;
            }

            int _change = 0;
            int _change1k = 0;
            int _change2k = 0;
            int _change5k = 0;
            int _change10k = 0;
            int _change20k = 0;
            int _change50k = 0;

            CalculateChange(_total, _payment, ref _change, ref _change1k, ref _change2k, ref _change5k, ref _change10k, ref _change20k, ref _change50k);
            CalculateStock();

            lbChanges.Text = _change.ToString();
            lb1k.Text = _change1k.ToString();
            lb2k.Text = _change2k.ToString();
            lb5k.Text = _change5k.ToString();
            lb10k.Text = _change10k.ToString();
            lb20k.Text = _change20k.ToString();
            lb50k.Text = _change50k.ToString();


            Reload();

        }

        public void CalculateStock()
        {
            foreach (VendingMenu _vendingMenu in vendingMachine.menus)
            {
                _vendingMenu.Stock -= _vendingMenu.Qty;
                _vendingMenu.Qty = 0;
                _vendingMenu.Total = 0;
            }

            lbPayment.Text = 0.ToString();

        }
        public void CalculateChange(int _total, int _payment, ref int _change, ref int _change1k, ref int _change2k, ref int _change5k, ref int _change10k, ref int _change20k, ref int _change50k)
        {
            _change = _payment - _total;
            int _tempChange = _change;
            if (_tempChange >= 50000)
            {
                _change50k = (int)Math.Ceiling( Convert.ToDouble(_tempChange / 50000));
                _tempChange -= _change50k * 50000;
            }

            if (_tempChange >= 20000)
            {
                _change20k = (int)Math.Ceiling(Convert.ToDouble(_tempChange / 20000));
                _tempChange -= _change20k * 20000;
            }

            if (_tempChange >= 10000)
            {
                _change10k = (int)Math.Ceiling(Convert.ToDouble(_tempChange / 10000));
                _tempChange -= _change10k * 10000;
            }

            if (_tempChange >= 5000)
            {
                _change5k = (int)Math.Ceiling(Convert.ToDouble(_tempChange / 5000));
                _tempChange -= _change5k * 5000;
            }

            if (_tempChange >= 2000)
            {
                _change2k = (int)Math.Ceiling(Convert.ToDouble(_tempChange / 2000));
                _tempChange -= _change2k * 2000;
            }

            if(_tempChange >= 1000)
            {
                _change1k = (int)Math.Ceiling(Convert.ToDouble(_tempChange / 1000));
            }

        }

        #region payment
        private void BtSub50k_Click(object sender, EventArgs e)
        {
            SubPayment("50000");
        }

        private void BtSub20k_Click(object sender, EventArgs e)
        {
            SubPayment("20000");
        }

        private void BtSub10k_Click(object sender, EventArgs e)
        {
            SubPayment("10000");
        }

        private void BtSub5k_Click(object sender, EventArgs e)
        {
            SubPayment("5000");
        }

        private void BtSub2k_Click(object sender, EventArgs e)
        {
            SubPayment("2000");
        }

        private void BtAdd50k_Click(object sender, EventArgs e)
        {
            AddPayment("50000");
        }

        private void BtAdd20k_Click(object sender, EventArgs e)
        {
            AddPayment("20000");
        }

        private void BtAdd10k_Click(object sender, EventArgs e)
        {
            AddPayment("10000");
        }

        private void BtAdd5k_Click(object sender, EventArgs e)
        {
            AddPayment("5000");
        }

        private void BtAdd2k_Click(object sender, EventArgs e)
        {
            AddPayment("2000");
        }
        #endregion

        private void AddPayment(string nominal)
        {
            int _result = Convert.ToInt32(lbPayment.Text) + Convert.ToInt32(nominal);
            if (_result < 0)
            {
                return;
            }

            lbPayment.Text = _result.ToString();
            Reload();
            
        }
        private void SubPayment(string nominal)
        {
            int _result = Convert.ToInt32(lbPayment.Text) - Convert.ToInt32(nominal);
            if (_result < 0)
            {
                return;
            }

            lbPayment.Text = _result.ToString();
            Reload();
        }
        private void BtAddStock_Click(object sender, EventArgs e)
        {
            Regex regex = new Regex(@"^\d+$");
            if (!regex.IsMatch(tbStock.Text))
            {
                tbStock.Text = "";
                return;
            }
            else if (string.IsNullOrWhiteSpace(cbMenu.Text))
            {
                MessageBox.Show($"Select Menu First");
                return;
            }

            vendingMachine.AddStock(cbMenu.Text, Convert.ToInt32(tbStock.Text));
            Reload();
        }

        private void TbStock_TextChanged(object sender, EventArgs e)
        {
            // Validate here.
            Regex regex = new Regex(@"^\d+$");
            if (!regex.IsMatch(tbStock.Text))
            {
                MessageBox.Show($"Please Input Integer Only!");
            }
        }

        #region menu
        private void BtMinCokelat_Click(object sender, EventArgs e)
        {
            vendingMachine.SubtractItem("Cokelat");
            Reload();
        }

        private void BtMinTango_Click(object sender, EventArgs e)
        {
            vendingMachine.SubtractItem("Tango");
            Reload();
        }

        private void BtMinOreo_Click(object sender, EventArgs e)
        {
            vendingMachine.SubtractItem("Oreo");
            Reload();
        }

        private void BtMinChips_Click(object sender, EventArgs e)
        {
            vendingMachine.SubtractItem("Chips");
            Reload();
        }

        private void BtMinBiskuit_Click(object sender, EventArgs e)
        {
            vendingMachine.SubtractItem("Biskuit");
            Reload();
        }

        private void BtCokelat_Click(object sender, EventArgs e)
        {
            vendingMachine.AddItem("Cokelat");
            Reload();
        }

        private void BtTango_Click(object sender, EventArgs e)
        {
            vendingMachine.AddItem("Tango");
            Reload();
        }

        private void BtOreo_Click(object sender, EventArgs e)
        {
            vendingMachine.AddItem("Oreo");
            Reload();
        }

        private void BtChips_Click(object sender, EventArgs e)
        {
            vendingMachine.AddItem("Chips");
            Reload();
        }

        private void BtBiskuit_Click(object sender, EventArgs e)
        {
            vendingMachine.AddItem("Biskuit");
            Reload();
        }
        #endregion

        public void Reload()
        {
            if (vendingMachine.menus.Count < 5)
            {
                MessageBox.Show($"Please Add All Menu First!");
                return;
            }

            vendingMachine.CalculateTotalPrice();

            lbBiskuit.Text = vendingMachine.menus.Where(x => x.Name == "Biskuit").Select(x => x.Qty).ToList().FirstOrDefault().ToString();
            lbChips.Text = vendingMachine.menus.Where(x => x.Name == "Chips").Select(x => x.Qty).ToList().FirstOrDefault().ToString();
            lbOreo.Text = vendingMachine.menus.Where(x => x.Name == "Oreo").Select(x => x.Qty).ToList().FirstOrDefault().ToString();
            lbTango.Text = vendingMachine.menus.Where(x => x.Name == "Tango").Select(x => x.Qty).ToList().FirstOrDefault().ToString();
            lbCokelat.Text = vendingMachine.menus.Where(x => x.Name == "Cokelat").Select(x => x.Qty).ToList().FirstOrDefault().ToString();

            lbStBiskuit.Text = vendingMachine.menus.Where(x => x.Name == "Biskuit").Select(x=>x.Stock).ToList().FirstOrDefault().ToString();
            lbStChips.Text = vendingMachine.menus.Where(x => x.Name == "Chips").Select(x => x.Stock).ToList().FirstOrDefault().ToString();
            lbStOreo.Text = vendingMachine.menus.Where(x => x.Name == "Oreo").Select(x => x.Stock).ToList().FirstOrDefault().ToString();
            lbStTango.Text = vendingMachine.menus.Where(x => x.Name == "Tango").Select(x => x.Stock).ToList().FirstOrDefault().ToString();
            lbStCokelat.Text = vendingMachine.menus.Where(x => x.Name == "Cokelat").Select(x => x.Stock).ToList().FirstOrDefault().ToString();

            lbTotal.Text = vendingMachine.TotalPrice.ToString();
        }

        public void OnLoad()
        {
            cbMenu.Items.Add("Biskuit");
            cbMenu.Items.Add("Chips");
            cbMenu.Items.Add("Oreo");
            cbMenu.Items.Add("Tango");
            cbMenu.Items.Add("Cokelat");

            vendingMachine.menus.Add(new VendingMenu
            {
                Id = 1,
                Name = "Biskuit",
                Price = 6000,
                Qty = 0,
                Total = 0,
                Stock = 5
            });
            vendingMachine.menus.Add(new VendingMenu
            {
                Id = 2,
                Name = "Chips",
                Price = 8000,
                Qty = 0,
                Total = 0,
                Stock = 5
            });
            vendingMachine.menus.Add(new VendingMenu
            {
                Id = 3,
                Name = "Oreo",
                Price = 10000,
                Qty = 0,
                Total = 0,
                Stock = 5
            });
            vendingMachine.menus.Add(new VendingMenu
            {
                Id = 4,
                Name = "Tango",
                Price = 12000,
                Qty = 0,
                Total = 0,
                Stock = 5
            });
            vendingMachine.menus.Add(new VendingMenu
            {
                Id = 5,
                Name = "Cokelat",
                Price = 15000,
                Qty = 0,
                Total = 0,
                Stock = 5
            });

            Reload();

        }



    }
}
