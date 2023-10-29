Your observation is correct: intercepting the SelectionChanged event won't work because everything important has already taken place. Here's one simple way that I think might work for you. The thing to understand is that you're not really editing the cell itself when you're editing. Your keystrokes are going into the `DataGrid.EditingControl` that is superimposed on top of the cell.

So, first I suggest handling the `dataGridView.EditingControlShowing` event by clearing its text (which has been copied into it from the actual cell).

```
dataGridView.EditingControlShowing += (sender, e) =>
{
    // Clear the editing control text.
    e.Control.Text = string.Empty;
};
```

[![blank cell to edit][1]][1]

Then handle the `dataGridView.CellValidating` event, examine the `e.FormattedValue` property and if it's blank, or if you don't like what you see there, cancel the edit. That 'should' put the text back to where it was before the edit.

```
dataGridView.CellValidating += (sender, e) =>
{
    if(string.IsNullOrWhiteSpace(e.FormattedValue.ToString()))
    {
        dataGridView.CancelEdit();
    }
};
```

[![revert cell][2]][2]

___

Here's the code I used to test this answer:

```
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
```

This example uses a bound data source with this `Record` class.

```
    class Record
    {
        public int Index { get; set; }
        public string BurstTime { get; set; } = "Write";
    }
}
```




  [1]: https://i.stack.imgur.com/6eJ4e.png
  [2]: https://i.stack.imgur.com/aK2c5.png