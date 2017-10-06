using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CargaIndividual.Clases;

namespace CargaIndividual
{
    public partial class LogDetalle : Form
    {
        public List<Log> ListaMensajes { get; set; }

        public LogDetalle()
        {
            InitializeComponent();
            ListaMensajes = new List<Log>();
        }

        private void LogDetalle_Load(object sender, EventArgs e)
        {
            this.DetalleMensajesGridView.DataSource = this.ListaMensajes;
        }
    }
}
