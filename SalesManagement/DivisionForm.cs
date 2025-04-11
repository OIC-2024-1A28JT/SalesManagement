using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SalesManagement.Common;
using SalesManagement.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesManagement
{
    public partial class DivisionForm : Form
    {
        //メッセージ表示用クラスのインスタンス化
        MessageDsp msg = new MessageDsp();

        public DivisionForm()
        {
            InitializeComponent();
        }

        private void DivisionForm_Load(object sender, EventArgs e)
        {
            //ボタンの初期設定
            fncButtonEnable(1);

            //データグリッドビューの列設定
            //列数の指定
            dataGridViewDsp.ColumnCount = 4;
            //0番目（左端）の列幅を設定
            dataGridViewDsp.Columns[0].Width = 70;
            //0番目（左端）の項目名を設定
            dataGridViewDsp.Columns[0].HeaderText = "部署ID";
            dataGridViewDsp.Columns[1].Width = 130;
            dataGridViewDsp.Columns[1].HeaderText = "部署名";
            dataGridViewDsp.Columns[2].Width = 70;
            dataGridViewDsp.Columns[2].HeaderText = "表示";
            dataGridViewDsp.Columns[3].Width = 250;
            dataGridViewDsp.Columns[3].HeaderText = "備考";
            //選択モードを行単位
            dataGridViewDsp.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //読み取り専用
            dataGridViewDsp.ReadOnly = true;
            // 行の追加と削除を禁止
            dataGridViewDsp.AllowUserToAddRows = false;
            dataGridViewDsp.AllowUserToDeleteRows = false;
            //全データ表示
            fncAllSelect();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            if (!SearchVerify())
            {
                return;
            }

            MDivision[] divisions = SearchGet();
            SearchDivision(divisions);
        }

        // 8.1.4.1
        private bool SearchVerify()
        {
            bool flg = true;

            if (!string.IsNullOrEmpty(textBoxDivisionID.Text))
            {
                flg = fncSelectInputCheck();
            }

            return flg;
        }

        // 8.1.4.2
        private MDivision[] SearchGet()
        {
            var context = new SalesContext();
            IQueryable<MDivision> divisions = context.MDivisions;

            if (!string.IsNullOrEmpty (textBoxDivisionID.Text))
            {
                divisions = divisions.Where(x => x.MDivisionId == int.Parse (textBoxDivisionID.Text));
            }

            if (!string.IsNullOrEmpty(textBoxDivisionName.Text))
            {
                divisions = divisions.Where(x => x.DivisionName.Contains (textBoxDivisionName.Text));
            }

            return divisions.ToArray();
        }

        // 8.1.4.3
        private void SearchDivision(MDivision[] divisions)
        {
            dataGridViewDsp.Rows.Clear();

            foreach (MDivision division in divisions)
            {
                dataGridViewDsp.Rows.Add(division.MDivisionId, division.DivisionName, division.DspFlg, division.Comments);
            }
        }

        private void fncAllSelect()
        {
            //全データの表示
            dataGridViewDsp.Rows.Clear();
            try
            {
                using var context = new SalesContext();
                foreach (var p in context.MDivisions)
                {
                    dataGridViewDsp.Rows.Add(p.MDivisionId, p.DivisionName, p.DspFlg, p.Comments);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //データグリッドビューからクリックされたデータをテキストボックスへ
            textBoxDivisionID.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[0].Value.ToString();
            textBoxDivisionName.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[1].Value.ToString();
            checkBoxDispFLG.Checked = (bool)dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[2].Value;
            textBoxComments.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[3].Value.ToString();
        }

        private void buttonRegist_Click(object sender, EventArgs e)
        {
            //string result;
            //if (RegistVerify())
            //{
            //    result = "true";
            //}
            //else
            //{
            //    result = "false";
            //}
            //MessageBox.Show(result);

            //MDivision? division = RegistGet();
            //if (division == null)
            //{
            //    MessageBox.Show("null");
            //}
            //else
            //{
            //    MessageBox.Show(
            //        string.Format("部署名：{0}\nフラグ：{1}\n備考：{2}",
            //        division.DivisionName, division.DspFlg,
            //        division.Comments));
            //}

            //MDivision division = new MDivision()
            //{
            //    MDivisionId = 1,
            //    DivisionName = "開発部",
            //    DspFlg = true,
            //    Comments = "システムを開発する部署です"
            //};
            //RegistDivision(division);

            if (!RegistVerify())  // ～がfalse→！～
            {
                return;           // 終了する→抜ければいい
            }

            MDivision? division = RegistGet();
            if (division == null)
            {
                return;
            }

            RegistDivision(division);
        }

        // 8.1.1.1
        private bool RegistVerify()
        {
            bool flg = fncAddInputCheck();
            return flg;
        }

        // 8.1.1.2
        private MDivision? RegistGet()
        {
            DialogResult result = msg.MsgDsp("M10001");
            if (result == DialogResult.Cancel)
            {
                return null;
            }

            string mName = textBoxDivisionName.Text;
            bool mFlg = checkBoxDispFLG.Checked;
            string mCom = textBoxComments.Text;
            MDivision division = new MDivision()
            {
                DivisionName = mName,
                DspFlg = mFlg,
                Comments = mCom
            };
            return division;
        }

        //8.1.1.3
        private void RegistDivision(MDivision division)
        {
            try
            {
                var context = new SalesContext();
                context.MDivisions.Add(division);
                context.SaveChanges();
                context.Dispose();
                fncAllSelect();
                msg.MsgDsp("M10002");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (!DeleteVerify())
            {
                return;
            }

            int? divisionId = DeleteGet();
            if (divisionId == null)
            {
                return;
            }

            DeleteDivision(divisionId.Value);
        }

        // 8.1.3.1
        private bool DeleteVerify()
        {
            bool flg = fncSelectInputCheck();

            return flg;
        }

        // 8.1.3.2
        private int? DeleteGet()
        {
            int mDivisionID = int.Parse(textBoxDivisionID.Text);

            DialogResult result = msg.MsgDsp("M10005", mDivisionID, textBoxDivisionName.Text);
            if (result == DialogResult.Cancel) 
            {
                return null;
            }
            return mDivisionID;
        }

        // 8.1.3.3
        private void DeleteDivision(int divisionId)
        {
            try
            {
                var context = new SalesContext();
                MDivision division = context.MDivisions.Single(x => x.MDivisionId == divisionId);
                context.MDivisions.Remove(division);
                context.SaveChanges();
                context.Dispose();
                fncAllSelect();
                msg.MsgDsp("M10006");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (!UpdateVerify())
            {
                return;
            }

            MDivision? division = UpdateGet();
            if (division == null)
            {
                return;
            }

            UpdateDivision(division);
        }

        // 8.1.2.1
        private bool UpdateVerify()
        {
            bool flg = fncSelectInputCheck();

            if (!flg)
            {
                return false;
            }

            flg = fncSelectInputCheck();

            return flg;
        }

        // 8.1.2.2
        private MDivision? UpdateGet()
        {
            DialogResult result = msg.MsgDsp("M10003",
                int.Parse(textBoxDivisionID.Text),
                textBoxDivisionName.Text);
            if (result == DialogResult.Cancel) 
            {
                return null;
            }

            int mDivisionID = int.Parse(textBoxDivisionID.Text);
            string mName = textBoxDivisionName.Text;
            bool mFlg = checkBoxDispFLG.Checked;
            string mCom = textBoxComments.Text;
            MDivision division = new MDivision()
            {
                MDivisionId = mDivisionID, DivisionName = mName,
                DspFlg = mFlg, Comments = mCom };
            return division;
        }

        // 8.1.2.3
        private void UpdateDivision(MDivision division)
        {
            try
            {
                var context = new SalesContext();
                MDivision updateDivision = context.MDivisions.Single(x => x.MDivisionId == division.MDivisionId);
                updateDivision.DivisionName = division.DivisionName;
                updateDivision.DspFlg = division.DspFlg;
                updateDivision.Comments = division.Comments;
                context.SaveChanges();
                context.Dispose();
                fncAllSelect();
                msg.MsgDsp("M10004");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void buttonFormDivisionClose_Click(object sender, EventArgs e)
        {
            // フォームを閉じる確認メッセージの表示
            DialogResult result = msg.MsgDsp("M10008");

            if (result == DialogResult.OK)
            {
                // OKの時の処理
                Close();
            }
            else
            {
                // キャンセルの時の処理
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBoxDivisionID.Text = "";
            textBoxDivisionName.Text = "";
            checkBoxDispFLG.Checked = true;
            textBoxComments.Text = "";
            dataGridViewDsp.Rows.Clear();
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            string str = Microsoft.VisualBasic.Interaction.InputBox("部署マスタを新規に作成する場合は「1」\r\n" +
                                                            "部署マスタにデータを追加する場合は「2」\r\n" +
                                                            "部署マスタへの追加をキャンセルする場合は「3」\r\n" +
                                                            "を入力してください", "部署マスタデータ追加確認", "3", -1, -1);
            //入力データのチェック
            if (str != "1" && str != "2" && str != "3")
            {
                //エラーメッセージ表示
                _ = msg.MsgDsp("M10011");
                return;
            }

            if (str == "3")
            {
                return;
            }

            try
            {
                if (str == "1")
                {
                    using var context = new SalesContext();
                    //M_Divisionテーブルを再作成
                    context.Database.ExecuteSqlRaw("TRUNCATE TABLE M_Division;");
                    context.SaveChanges();
                }

                //インポートするCSVファイルの指定
                string csvpth = Path.Combine(Environment.CurrentDirectory, "Division.csv");
                //データテーブルの設定
                DataTable dt = new DataTable();
                dt.TableName = "M_Division";

                //csvファイルの内容をDataTableへ
                // 全行読み込み
                var rows = File.ReadAllLines(csvpth, Encoding.GetEncoding("Shift-JIS")).Select(x => x.Split(','));
                // 列設定
                dt.Columns.AddRange(rows.First().Select(s => new DataColumn(s)).ToArray());
                // 行追加
                foreach (var row in rows.Skip(1))
                {
                    dt.Rows.Add(row);
                }

                //DB接続情報の取得
                var dbpth = System.Configuration.ConfigurationManager.ConnectionStrings["SalesContext"].ConnectionString;
                //DataTableの内容をDBへ追加
                using (var bulkCopy = new SqlBulkCopy(dbpth))
                {
                    bulkCopy.DestinationTableName = dt.TableName;
                    bulkCopy.WriteToServer(dt);
                }

                //データグリッドに全データ表示
                fncAllSelect();
                //完了メッセージの表示
                _ = msg.MsgDsp("M10007");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxDivisionID_TextChanged(object sender, EventArgs e)
        {
            //部署IDの有無をチェック
            if (string.IsNullOrEmpty(textBoxDivisionID.Text))
            {
                fncButtonEnable(1);
            }
            else
            {
                fncButtonEnable(2);
            }
        }

        ///////////////////////////////
        //メソッド名：fncButtonEnable()
        //引　数   ：int型：chk　（部署IDの入力状況）
        //戻り値   ：なし
        //機　能   ：部署IDの入力状況に応じてボタンの
        //          ：enableプロパティの設定
        ///////////////////////////////
        private void fncButtonEnable(int chk)
        {
            //chkが１ならば、部署IDが空、２ならば有
            if (chk == 1)
            {
                //Enableは、コントロールの使用可否
                buttonRegist.Enabled = true;
                buttonSearch.Enabled = true;
                buttonPrint.Enabled = true;
                buttonClear.Enabled = true;
                buttonClose.Enabled = true;
                buttonLogout.Enabled = true;
                buttonImport.Enabled = true;
                buttonExport.Enabled = true;
                buttonUpdate.Enabled = false;
                buttonDelete.Enabled = false;
            }
            else if (chk == 2)
            {
                buttonRegist.Enabled = false;
                buttonSearch.Enabled = true;
                buttonPrint.Enabled = true;
                buttonClear.Enabled = true;
                buttonClose.Enabled = true;
                buttonLogout.Enabled = true;
                buttonImport.Enabled = true;
                buttonExport.Enabled = true;
                buttonUpdate.Enabled = true;
                buttonDelete.Enabled = true;
            }
        }

        ///////////////////////////////
        //メソッド名：fncAddInputCheck()
        //引　数   ：なし
        //戻り値   ：true(エラーなし） Or false（エラーあり）
        //機　能   ：入力データのチェック
        ///////////////////////////////
        private bool fncAddInputCheck()
        {
            //部署名の入力データ有無チェック
            if (string.IsNullOrWhiteSpace(textBoxDivisionName.Text))
            {
                //入力無しエラーメッセージの表示
                _ = msg.MsgDsp("M10009");
                textBoxDivisionName.Focus();
                return false;
            }

            //部署名の文字数チェック
            if (textBoxDivisionName.Text.Length > 50)
            {
                //文字数エラーメッセージの表示
                _ = msg.MsgDsp("M10010");
                textBoxDivisionName.Focus();
                return false;
            }

            return true;
        }

        ///////////////////////////////
        //メソッド名：fncSelectInputCheck()
        //引　数   ：なし
        //戻り値   ：true(データあり） Or false（データなし）
        //機　能   ：部署IDの存在チェック
        ///////////////////////////////
        private bool fncSelectInputCheck()
        {
            bool flg = false;
            //部署IDの存在チェック
            if (int.TryParse(textBoxDivisionID.Text, out int divisionId))
            {
                try
                {
                    using var context = new SalesContext();
                    //部署IDが存在するか
                    flg = context.MDivisions.Any(x => x.MDivisionId == divisionId);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            //部署IDが存在しなかった場合のメッセージ表示
            if (!flg)
            {
                _ = msg.MsgDsp("M10012");
                textBoxDivisionID.Focus();
            }
            return flg;
        }
    }
}
