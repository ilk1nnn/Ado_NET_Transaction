using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //using (var connection = new SqlConnection())
            //{

            //    connection.ConnectionString = App.ConnectionStringSql;
            //    connection.Open();
            //    DataSet set = new DataSet();
            //    var da = new SqlDataAdapter("SELECT Id,Name,Pages,YearPress FROM Books",connection);

            //    da.Fill(set,"Books");

            //    DataViewManager dvm = new DataViewManager(set);
            //    dvm.DataViewSettings["Books"].RowFilter = "Pages>500";
            //    //dvm.DataViewSettings["Books"].RowFilter = "YearPress>2000";
            //    //dvm.DataViewSettings["Books"].Sort = "YearPress DESC";

            //    DataView dv = dvm.CreateDataView(set.Tables["Books"]);


            //    data_grid.ItemsSource = dv;

            //    //data_grid.ItemsSource = set.Tables[0].DefaultView;

            //}



            // ------------------------------------------------------------------------------------------


            #region Transaction

            SqlTransaction sqlTransaction = null;
            using (var connection = new SqlConnection())
            {

                connection.ConnectionString = App.ConnectionStringSql;
                connection.Open();


                sqlTransaction = connection.BeginTransaction();

                SqlCommand comm1 = new SqlCommand("INSERT INTO Press(Id,Name) VALUES(@id,@name)",connection);

                comm1.Transaction = sqlTransaction;

                comm1.Parameters.Add(new SqlParameter()
                {
                    SqlDbType = SqlDbType.Int,
                    Value = 5555,
                    ParameterName = "@id"
                });

                comm1.Parameters.Add(new SqlParameter()
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Value = "John",
                    ParameterName = "@name"
                });


                SqlCommand comm2 = new SqlCommand("sp_UpdateBook",connection);
                comm2.Transaction = sqlTransaction;
                comm2.CommandType = CommandType.StoredProcedure;

                var p1 = new SqlParameter();
                p1.Value = 1;
                p1.ParameterName = "@My_Id";
                p1.SqlDbType = SqlDbType.Int;

                var p2 = new SqlParameter();
                p2.Value = 2345;
                p2.ParameterName = "@Page";
                p2.SqlDbType = SqlDbType.Int;


                comm2.Parameters.Add(p1);
                comm2.Parameters.Add(p2);



                try
                {
                    comm1.ExecuteNonQuery();
                    comm2.ExecuteNonQuery();
                    sqlTransaction.Commit();
                    MessageBox.Show("OKAY");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    sqlTransaction.Rollback();
                }



            }


                #endregion

            }
    }
}
