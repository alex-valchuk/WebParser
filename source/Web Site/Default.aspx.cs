using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Searcher.Lib;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Text;
using System.Collections;
using System.Data;
using UrlFinder.Log;

namespace Searcher.UI
{
    public partial class Default : System.Web.UI.Page
    {
        #region Fields

        private ShopResult searchResult = null;

        #endregion

        #region Methods

        #region Event Handlers

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }


        protected void OnFindClickHandler(object sender, EventArgs e)
        {
            result.Text = "";
            searchResult = null;
            String inputFilePath = Server.MapPath(String.Format("xml/input/{0}.xml", addressList.Text));
            String outputFilePath = Server.MapPath(String.Format("xml/output/{0}({1}).xml", addressList.Text, GetCurrentDateTimeString()));

            using (FileStream input = File.Open(inputFilePath, FileMode.Open))
            using (FileStream output = File.Create(outputFilePath))
            {
                try
                {
                    //LogInfo log = new LogInfo(Server.MapPath("logs/"));
                    SearchProcess process = new SearchProcess(input, output);
                    searchResult = (ShopResult)process.GetSearchResult(new ShopProductFactory(), GetParamList());//, log);
                }
                catch(Exception ex)
                {
                    input.Close();
                    output.Close();
                    LogInfo log = new LogInfo(Server.MapPath("logs/"));
                    log.Write(ex);
                }
            }
            this.WriteResultDetails();
        }


        protected void OnGetResultClick(object sender, EventArgs e)
        {
            String path = Server.MapPath(String.Format("xml/output/{0}.xml", fileListBox.SelectedValue));
            if (File.Exists(path))
            {
                using (FileStream input = File.Open(path, FileMode.Open))
                {
                    try
                    {
                        searchResult = (ShopResult)XmlHelper.Deserialize(input, typeof(ShopResult));
                    }
                    catch(Exception ex)
                    {
                        message.Text = "File inaccessible";
                        LogInfo log = new LogInfo(Server.MapPath("logs/"));
                        log.Write(ex);
                    }
                }
            }
            if (sender == getHtml)
                this.GetHtml();
            else if (sender == getXml)
                this.GetXml(path);
            this.WriteResultDetails();
        }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            this.BindFileListBox();
            this.BindAddressList();
        }


        #endregion

        #region Helpers

        private void WriteResultDetails()
        {
            if (searchResult != null)
            {
                message.Text = String.Format("{0} items are found.", searchResult.ProductList.Count);
            }
        }


        private void BindAddressList()
        {
            if (!IsPostBack)
            {
                addressList.DataSource = CreateDataSourceByPath("xml/input/");
                addressList.DataTextField = "AddressTextField";
                addressList.DataValueField = "AddressValueField";
                addressList.DataBind();
            }
        }


        private void BindFileListBox()
        {
            int selectedIndex = fileListBox.SelectedIndex;
            fileListBox.DataSource = CreateDataSourceByPath("xml/output/");
            fileListBox.DataTextField = "AddressTextField";
            fileListBox.DataValueField = "AddressValueField";
            fileListBox.SelectedIndex = selectedIndex;
            fileListBox.DataBind();
            if (fileListBox.SelectedIndex < 0)
                fileListBox.SelectedIndex = 0;
            if (fileListBox.Items.Count == 0)
                this.SetButtonState(false);
            else
                this.SetButtonState(true);
        }


        private void SetButtonState(bool value)
        {
            getHtml.Enabled = value;
            getXml.Enabled = value;
        }


        private String GetCurrentDateTimeString()
        {
            String time = DateTime.Now.ToString();
            time = Regex.Replace(time, "[/:]", ".");
            return time;
        }


        private IList<String> GetParamList()
        {
            if (!String.IsNullOrEmpty(param.Text))
            {
                return param.Text.Split(',');
            }
            return null;
        }


        /// <summary>
        /// Return Xml file to response
        /// </summary>
        /// <param name="path"> the path of returned xml file </param>
        private void GetXml(String path)
        {
            if (searchResult != null)
            {
                Response.Clear();
                Response.ContentType = "text/xml";
                Response.BinaryWrite(File.ReadAllBytes(path));
                Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}", "result.xml"));
                Response.End();
            }
        }


        /// <summary>
        /// Generate HTML by resalt of searching for
        /// </summary>
        private void GetHtml()
        {
            result.Text = "";
            if (searchResult != null)
            {
                foreach (ShopProduct product in searchResult.ProductList)
                {
                    result.Text += "<table border='1'><tr>";
                    result.Text += String.Format("<td><a href='{0}' alt='{1}'>{1}</a></td></tr><tr><td><img src='{2}' alt='{1}'></img></td></tr><tr><td>Цена: ", product.Address, product.Title, product.Image);
                    foreach (String price in product.PriceList)
                    {
                        result.Text += String.Format("{0} | ", price);
                    }
                    result.Text += "</td></tr>";
                    result.Text += String.Format("<tr><td>{0}</td></tr>", product.Description);
                    result.Text += "</table><br /><br />";
                }
            }
        }


        /// <summary>
        /// Create DataSource by files
        /// </summary>
        /// <returns> ICollection </returns>
        protected ICollection CreateDataSourceByPath(String path)
        {
            DataTable table = new DataTable();

            table.Columns.Add(new DataColumn("AddressTextField", typeof(String)));
            table.Columns.Add(new DataColumn("AddressValueField", typeof(String)));

            IList<String> fileList = FileHelper.GetFileNamesFomPath(Server.MapPath(path));
            foreach (String file in fileList)
            {
                DataRow row = table.NewRow();
                row[0] = file;
                row[1] = file;
                table.Rows.Add(row);
            }
            DataView dv = new DataView(table);
            return dv;
        }

        #endregion

        #endregion
    }
}
