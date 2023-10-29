using System.ComponentModel;

namespace dgv_with_revert_non_commit
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            dataGridView.DataSource = DataSource;

            dataGridView.CellValidating += (sender, e) =>
            {
                if(string.IsNullOrWhiteSpace(e.FormattedValue.ToString()))
                {
                    dataGridView.CancelEdit();
                }
            };
            dataGridView.EditingControlShowing += (sender, e) =>
            {
                // Clear the editing control text.
                e.Control.Text = string.Empty;
            };

            for (int i = 0; i < 10; i++)
            {
                DataSource.Add(new Record { Index = i });
            }
        }
        readonly BindingList<Record> DataSource = new BindingList<Record>();
    }

    class Record
    {
        public int Index { get; set; }
        public string BurstTime { get; set; } = "Write";
    }
}