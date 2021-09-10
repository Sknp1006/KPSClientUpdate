using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace AutoUpdater.UpdateHelper
{
	/// <summary>
	/// 智能升级UI窗体
	/// </summary>
	public class FormUpdate : System.Windows.Forms.Form
	{
		#region 系统生成

		private System.Windows.Forms.Panel pnlTop;
		private System.Windows.Forms.Panel pnlBottom;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Panel pnlLeft;
		private System.Windows.Forms.PictureBox pcbContainer;
		private System.Windows.Forms.Panel pnlMain;
		private System.Windows.Forms.Label lblIntro;
		private System.Windows.Forms.RichTextBox rchText;
		private System.Windows.Forms.Panel pnlContainer;
		private System.Windows.Forms.ListBox lsbDownloadList;
		private System.Windows.Forms.ProgressBar prgBar;
		private System.Windows.Forms.CheckBox chkAgree;
		private System.Windows.Forms.StatusBar m_StatusBar;
		private System.Windows.Forms.StatusBarPanel sp_DownFileName;
		private System.Windows.Forms.StatusBarPanel sp_DownloadInfo;
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="obj">更新对象</param>
		/// <param name="appilcation">更新完成后应启动的应用程序</param>
		public FormUpdate( UpdateUtility obj )
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			InitializeComponent();
			this.objUpdate = obj;
			//
			// TODO: 在 InitializeComponent 调用后添加任何构造函数代码
			//
		}


		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows 窗体设计器生成的代码
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
            this.pnlTop = new System.Windows.Forms.Panel();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.pcbContainer = new System.Windows.Forms.PictureBox();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.chkAgree = new System.Windows.Forms.CheckBox();
            this.prgBar = new System.Windows.Forms.ProgressBar();
            this.lsbDownloadList = new System.Windows.Forms.ListBox();
            this.rchText = new System.Windows.Forms.RichTextBox();
            this.lblIntro = new System.Windows.Forms.Label();
            this.m_StatusBar = new System.Windows.Forms.StatusBar();
            this.sp_DownFileName = new System.Windows.Forms.StatusBarPanel();
            this.sp_DownloadInfo = new System.Windows.Forms.StatusBarPanel();
            this.pnlBottom.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbContainer)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.pnlContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sp_DownFileName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sp_DownloadInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Enabled = false;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(594, 61);
            this.pnlTop.TabIndex = 1;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnNext);
            this.pnlBottom.Controls.Add(this.btnClose);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 404);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(594, 37);
            this.pnlBottom.TabIndex = 2;
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Location = new System.Drawing.Point(433, 7);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 0;
            this.btnNext.Text = "下一步";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(513, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.pcbContainer);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 61);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(180, 343);
            this.pnlLeft.TabIndex = 3;
            // 
            // pcbContainer
            // 
            this.pcbContainer.Image = global::KPSClientUpdate.Properties.Resources.loginform_left_showe;
            this.pcbContainer.InitialImage = global::KPSClientUpdate.Properties.Resources.loginform_left_showe;
            this.pcbContainer.Location = new System.Drawing.Point(3, 0);
            this.pcbContainer.Name = "pcbContainer";
            this.pcbContainer.Size = new System.Drawing.Size(176, 343);
            this.pcbContainer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcbContainer.TabIndex = 0;
            this.pcbContainer.TabStop = false;
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.pnlContainer);
            this.pnlMain.Controls.Add(this.lblIntro);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(180, 61);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(414, 343);
            this.pnlMain.TabIndex = 4;
            // 
            // pnlContainer
            // 
            this.pnlContainer.Controls.Add(this.chkAgree);
            this.pnlContainer.Controls.Add(this.prgBar);
            this.pnlContainer.Controls.Add(this.lsbDownloadList);
            this.pnlContainer.Controls.Add(this.rchText);
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContainer.Location = new System.Drawing.Point(0, 32);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Size = new System.Drawing.Size(414, 311);
            this.pnlContainer.TabIndex = 2;
            // 
            // chkAgree
            // 
            this.chkAgree.Location = new System.Drawing.Point(15, 195);
            this.chkAgree.Name = "chkAgree";
            this.chkAgree.Size = new System.Drawing.Size(104, 24);
            this.chkAgree.TabIndex = 3;
            this.chkAgree.Text = "同意以上条款";
            this.chkAgree.Visible = false;
            this.chkAgree.CheckedChanged += new System.EventHandler(this.chkAgree_CheckedChanged);
            // 
            // prgBar
            // 
            this.prgBar.Location = new System.Drawing.Point(9, 157);
            this.prgBar.Name = "prgBar";
            this.prgBar.Size = new System.Drawing.Size(24, 23);
            this.prgBar.TabIndex = 2;
            this.prgBar.Visible = false;
            // 
            // lsbDownloadList
            // 
            this.lsbDownloadList.HorizontalScrollbar = true;
            this.lsbDownloadList.ItemHeight = 12;
            this.lsbDownloadList.Location = new System.Drawing.Point(9, 73);
            this.lsbDownloadList.Name = "lsbDownloadList";
            this.lsbDownloadList.Size = new System.Drawing.Size(120, 76);
            this.lsbDownloadList.TabIndex = 1;
            // 
            // rchText
            // 
            this.rchText.BackColor = System.Drawing.SystemColors.Control;
            this.rchText.Location = new System.Drawing.Point(0, 0);
            this.rchText.Name = "rchText";
            this.rchText.ReadOnly = true;
            this.rchText.Size = new System.Drawing.Size(82, 55);
            this.rchText.TabIndex = 0;
            this.rchText.Text = "文本显示控件";
            this.rchText.Visible = false;
            // 
            // lblIntro
            // 
            this.lblIntro.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblIntro.Location = new System.Drawing.Point(0, 0);
            this.lblIntro.Name = "lblIntro";
            this.lblIntro.Size = new System.Drawing.Size(414, 32);
            this.lblIntro.TabIndex = 1;
            this.lblIntro.Text = "Intro";
            this.lblIntro.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_StatusBar
            // 
            this.m_StatusBar.Location = new System.Drawing.Point(180, 382);
            this.m_StatusBar.Name = "m_StatusBar";
            this.m_StatusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.sp_DownFileName,
            this.sp_DownloadInfo});
            this.m_StatusBar.ShowPanels = true;
            this.m_StatusBar.Size = new System.Drawing.Size(414, 22);
            this.m_StatusBar.SizingGrip = false;
            this.m_StatusBar.TabIndex = 5;
            this.m_StatusBar.Visible = false;
            // 
            // sp_DownFileName
            // 
            this.sp_DownFileName.Name = "sp_DownFileName";
            this.sp_DownFileName.Width = 150;
            // 
            // sp_DownloadInfo
            // 
            this.sp_DownloadInfo.Name = "sp_DownloadInfo";
            this.sp_DownloadInfo.Width = 200;
            // 
            // FormUpdate
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(594, 441);
            this.Controls.Add(this.m_StatusBar);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormUpdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "软件智能更新";
            this.Load += new System.EventHandler(this.FormUpdate_Load);
            this.pnlBottom.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pcbContainer)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.pnlContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sp_DownFileName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sp_DownloadInfo)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion		

		#endregion

		#region 字段

		/// <summary>
		/// 更新时需关闭的关联应用程序列表
		/// </summary>
		private ArrayList alCloseApplications;

		/// <summary>
		/// 更新步骤
		/// </summary>
		/// <remarks>
		///		1:初始化更新信息
		///		2:显示协议信息
		///		3:显示更新历史记录信息
		///		4:下载更新文件
		///		5:更新信息本地信息
		///		6:显示更新完成信息
		///		7:执行主程序，退出更新程序
		/// </remarks>
		private int nStep;

		/// <summary>
		/// 下载对象
		/// </summary>
		private UpdateUtility objUpdate;

		/// <summary>
		/// 更新是否成功
		/// </summary>
		private bool bSussful;

		#endregion

		#region UI事件

		/// <summary>
		/// 初始化数据
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FormUpdate_Load(object sender, System.EventArgs e)  // 启动界面
		{
			this.bSussful = false;
            this.lblIntro.Text = "单击[下一步]下载更新:";
            try
			{
				if( this.objUpdate.LocalUpdateUrlConfigInfo != null )
				{
					//this.Text = string.Format( "更新[{0}]--软件智能更新",this.objUpdate.LocalUpdateUrlConfigInfo.strUpdateTitle );
					this.Text = string.Format("鲲鹏扫描端升级向导", this.objUpdate.LocalUpdateUrlConfigInfo.strUpdateTitle);
				}				
				this.alCloseApplications = Global.ParseCloseApplications( this.objUpdate.RemoteConfigInfo );
				this.nStep = 4;  // 从第四步开始
				//设置事件
				this.objUpdate.DeleteTempFileComplete += new EventHandler(objUpdate_DeleteTempFileComplete);
				this.objUpdate.DownloadLicenceInfoComplete += new TextEventHandler(objUpdate_DownloadLicenceInfoComplete);
				this.objUpdate.DownloadFileBlockComplete += new DownloadFileEventHandler(objUpdate_DownloadFileBlockComplete);
				this.objUpdate.DownloadFileComplete += new TextEventHandler(objUpdate_DownloadFileComplete);
				this.objUpdate.DownloadFileOver += new EventHandler(objUpdate_DownloadFileOver);
				this.objUpdate.DownloadHistoryInfoComplete += new TextEventHandler(objUpdate_DownloadHistoryInfoComplete);
				this.objUpdate.InializationDownloadInfoComplete += new ArrayListEventHandler(objUpdate_InializationDownloadInfoComplete);
				this.objUpdate.InitlizationDownloadFile += new DownloadFileInitEventHandler(objUpdate_InitlizationDownloadFile);
				this.objUpdate.PrepareDownloadFiles += new ArrayListEventHandler(objUpdate_PrepareDownloadFiles);
				this.objUpdate.UpdateComplete += new EventHandler(objUpdate_UpdateComplete);
				this.objUpdate.UpdateLocalConfigInfoComplete += new EventHandler(objUpdate_UpdateLocalConfigInfoComplete);
				this.objUpdate.UpdateLocalFilesComplete += new EventHandler(objUpdate_UpdateLocalFilesComplete);
                this.ViewRichTextBox(false, false);
                this.m_StatusBar.Visible = false;
                this.ViewListBox(false, false);  // 第一个参数设置为false
                this.FillCloseApplication();

                this.StepTwo();
                //this.StepFour();
            }
			catch( Exception ex )
			{
				this.DisplayExceptionInfo( ex );
			}
		}

		


		/// <summary>
		/// 下一步
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnNext_Click(object sender, System.EventArgs e)
		{	
			switch( this.nStep )
			{
                //case 2:
                //    this.StepTwo();  // 显示协议
                //    break;
                //case 3:
                //    this.StepThree();  // 显示更新历史记录信息
                //    break;
                case 4:
					this.StepFour();  // 下载更新文件
					break;
                case 5:
                    this.StepFive();  // 更新信息本地信息
                    break;
                case 6:
                    this.DisplayUpdateCompleteInfo();  // 显示更新完成信息
                    break;
                case 7:
					this.RunMainProgram();  // 运行主程序
					break;					
			}
		}

		
		/// <summary>
		/// 关闭
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnClose_Click(object sender, System.EventArgs e)
		{
			if( MessageBox.Show( "确认要停止智能更新吗？", "系统询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) == DialogResult.Yes )
			{
				Application.Exit();
			}
		}


		/// <summary>
		/// 处理下一步按钮状态
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void chkAgree_CheckedChanged(object sender, System.EventArgs e)
		{
			this.btnNext.Enabled = this.chkAgree.Checked;
		}


		#endregion

		#region 处理事件

		/// <summary>
		/// 删除临时文件完成
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void objUpdate_DeleteTempFileComplete( object sender,EventArgs e )
		{
			this.rchText.Text += "\r\n删除临时文件完成.";
		}


		/// <summary>
		/// 下载更新文件结束
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void objUpdate_DownloadFileOver( object sender,EventArgs e )
		{
			this.lblIntro.Text = "下载更新文件完成.";
			this.ViewStatusBar( false );
			this.ViewProgressBar( false );
			this.btnNext.Enabled = true;			
		}


		/// <summary>
		/// 更新完成
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void objUpdate_UpdateComplete( object sender,EventArgs e )
		{	
			this.lblIntro.Text = "下载完成";			
			this.rchText.Text = string.Format( "应用程序版本:{0}\r\n最后更新日期:{1:yyyy年MM月dd日}",this.objUpdate.RemoteConfigInfo.UpdateMainVersion,this.objUpdate.RemoteConfigInfo.UpdateDate );
			this.btnNext.Enabled = true;
			this.nStep = 6;
			this.bSussful = true;
		}


		/// <summary>
		/// 更新本地配置信息完成
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void objUpdate_UpdateLocalConfigInfoComplete( object sender,EventArgs e )
		{
			this.rchText.Text += "\r\n更新本地配置信息完成.\r\n正在删除临时文件...";
		}


		/// <summary>
		/// 更新本地文件完成
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void objUpdate_UpdateLocalFilesComplete( object sender,EventArgs e )
		{
			this.rchText.Text += "\r\n更新本地文件完成.\r\n正在更新本地配置信息...";						
		}


		/// <summary>
		/// 下载协议内容完成事件
		/// </summary>
		/// <param name="e"></param>
		private void objUpdate_DownloadLicenceInfoComplete( TextEventArgs e )
		{
			this.lblIntro.Text = "用户协议条款:";
			this.ViewRichTextBox( true,true );
			this.rchText.Text = e.Text;
//			this.btnNext.Enabled = true;
		}


		/// <summary>
		/// 下载更新记录信息完成事件
		/// </summary>
		/// <param name="e"></param>
		private void objUpdate_DownloadHistoryInfoComplete( TextEventArgs e )
		{
			this.lblIntro.Text = "更新历史记录:";
			this.ViewRichTextBox( true,false );
			this.rchText.Text = e.Text;
			this.btnNext.Enabled = true;
		}


		/// <summary>
		/// 下载文件完成
		/// </summary>
		/// <param name="e"></param>
		private void objUpdate_DownloadFileComplete( TextEventArgs e )
		{
			this.prgBar.Value = 0;
			this.prgBar.Maximum = 0;
			this.prgBar.Invalidate();
			this.sp_DownFileName.Text = string.Empty;
			this.sp_DownloadInfo.Text = string.Empty;
		}


		/// <summary>
		/// 下载文件块完成
		/// </summary>
		/// <param name="e"></param>
		private void objUpdate_DownloadFileBlockComplete( UpdateHelper.DownloadFileEventArgs e ) 
		{
			this.prgBar.Value = e.Downloads;
			this.sp_DownloadInfo.Text = string.Format( "{0}/{1}",e.Downloads,e.FileSize );
		}


		/// <summary>
		/// 初始化下载信息完成事件
		/// </summary>
		/// <param name="e"></param>
		private void objUpdate_InializationDownloadInfoComplete( UpdateHelper.ArrayListEventArgs e )
		{
		}


		/// <summary>
		/// 准备下载文件事件
		/// </summary>
		/// <param name="e"></param>
		private void objUpdate_PrepareDownloadFiles( UpdateHelper.ArrayListEventArgs e )
		{
			//填充下载文件列表
			this.lsbDownloadList.Items.Clear();
			foreach( FileDescription info in e.DataList )
			{
				this.lsbDownloadList.Items.Add( info );
			}
			Application.DoEvents();
		}


		/// <summary>
		/// 准备开始下载文件事件
		/// </summary>
		/// <param name="e"></param>
		private void objUpdate_InitlizationDownloadFile( UpdateHelper.DownloadFileInitEventArgs e )
		{
			//准备下载文件
            this.sp_DownFileName.Text = string.Format( "{0}[{1}KB]",e.FileName,Global.GetFileSizeOfKB( e.FileSize ) );
			this.prgBar.Maximum = e.FileSize;
		}


		#endregion

		#region 方法

		/// <summary>
		/// 显示协议信息
		/// </summary>
		//private void StepTwo()
		//{
		//	try
		//	{
		//		if( !this.CheckApplicationsIsClosed() )
		//		{
		//			MessageBox.Show( "请先关闭关联的应用程序！","系统提示" );
		//			return;
		//		}
		//		this.btnNext.Enabled = false;
		//		this.lblIntro.Text = "正在下载协议信息...";
		//		//显示协议信息
		//		this.objUpdate.DisplayLicenceInfo();
		//		Application.DoEvents();
		//		this.nStep = 3;
		//	}
		//	catch( Exception ex )
		//	{
		//		this.btnNext.Enabled = true;
		//		this.DisplayExceptionInfo( ex );
		//	}
		//}
		private void StepTwo()
		{
			try
			{
				while (true)
    		    {
					if (!this.CheckApplicationsIsClosed())
                    {
						DialogResult answer = MessageBox.Show("有正在运行的扫描端！是否关闭？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
						if (answer == DialogResult.OK)
						{
							// 杀死进程
							Process[] p = Process.GetProcessesByName("KPScanClient");
							p[0].Kill();
							return;

						}
						else if (answer == DialogResult.Cancel)
						{
							return;
						}
					}
                    else
                    {
						break;
                    }
				}

				this.nStep = 4;  // 下一步序号
			}
			catch (Exception ex)
			{
				this.btnNext.Enabled = true;
				this.DisplayExceptionInfo(ex);
			}
		}


		/// <summary>
		/// 显示更新历史记录信息
		/// </summary>
		private void StepThree()
		{
			try
			{				
				this.lblIntro.Text = "正在下载更新历史记录...";
				this.btnNext.Enabled = false;
				//显示协议信息
				this.objUpdate.DisplayHistoryInfo();
//				Application.DoEvents();
				this.nStep = 4;
			}
			catch( Exception ex )
			{
				this.btnNext.Enabled = true;
				this.DisplayExceptionInfo( ex );
			}
		}


		/// <summary>
		/// 下载更新文件
		/// </summary>
		private void StepFour()
		{
			try
			{				
				this.lblIntro.Text = "正在下载更新文件...";
				this.ViewRichTextBox( false,false );
				this.ViewCheckBox( false );
				this.btnNext.Enabled = false;
				this.ViewListBox( true,true );
				this.ViewProgressBar( true );
				this.ViewStatusBar( true );	
				this.objUpdate.DownloadUpdateFiles();
				this.nStep = 5;
			}
			catch( Exception ex )
			{
				this.btnNext.Enabled = true;
				this.DisplayExceptionInfo( ex );
			}
		}


		/// <summary>
		/// 更新信息本地信息
		/// </summary>
		private void StepFive()
		{			
			this.ViewCheckBox( false );
			this.ViewListBox( false,false );
			this.ViewProgressBar( false );
			this.ViewRichTextBox( true,false );
			this.ViewStatusBar( false );
			this.lblIntro.Text = "正在更新本地信息,请稍候...";
			this.rchText.Text = "正在更新本地文件...";
			this.btnNext.Enabled = false;
			try
			{
				this.objUpdate.UpdateLocalFiles();
			}
			catch( Exception ex )
			{
				this.DisplayExceptionInfo( ex );
			}
		}


		/// <summary>
		/// 显示更新完成信息
		/// </summary>
		private void DisplayUpdateCompleteInfo()
		{
			this.rchText.Text = string.Format( "\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n    请单击[完成]以退出向导。" );
			this.btnNext.Text = "完成";
			this.nStep = 7;

			// 这里要回传
			this.objUpdate.callback();
		}


		/// <summary>
		/// 显示异常信息
		/// </summary>
		private void DisplayExceptionInfo( Exception ex )
		{
			this.lblIntro.Text = "更新失败:";
			this.btnNext.Text = "完成";
			this.ViewRichTextBox( true,false );
			this.ViewStatusBar( false );
            System.Text.StringBuilder mString = new System.Text.StringBuilder();
			//mString.Append( string.Format( "更新失败!原因如下:\r\n    {0}\r\n",ex.ToString() ) );
			//mString.Append( "    由于远程服务器的错误导致更新包无法下载，对此我们深感歉意。\r\n" );
			mString.Append("\r\n\r\n\r\n    由于远程服务器的错误导致更新包无法下载，请稍后重试。\r\n");
			this.rchText.Text = mString.ToString();
			this.nStep = 7;
		}

         
		/// <summary>
		/// 运行主程序
		/// </summary>
		private void RunMainProgram()
		{
			//更新成功，运行主程序
			if( this.bSussful && this.objUpdate.LocalUpdateUrlConfigInfo.ApplicationFileName != null && this.objUpdate.LocalUpdateUrlConfigInfo.ApplicationFileName.Length > 0 )
			{
				System.Diagnostics.Process.Start( string.Format( "{0}\\{1}",Global.AssemblyPath,this.objUpdate.LocalUpdateUrlConfigInfo.ApplicationFileName ),this.objUpdate.LocalUpdateUrlConfigInfo.Parameters);
			}
			Application.Exit();
		}


		/// <summary>
		/// 检查关联应用程序是否关闭
		/// </summary>
		/// <returns></returns>
		private bool CheckApplicationsIsClosed()
		{
			bool bReturn = true;
			foreach( Process info in Process.GetProcesses() )  
			{
				foreach( string file in this.lsbDownloadList.Items )
				{
					if( Path.GetFileNameWithoutExtension( file ).ToLower() == info.ProcessName.ToLower() )
					{
						bReturn = false;
						break;
					}
				}
				if( !bReturn )
				{
					break;
				}
			}
			return bReturn;
		}


		/// <summary>
		/// 显示应关闭的应用程序列表
		/// </summary>
		private void FillCloseApplication()
		{
			this.lsbDownloadList.Items.Clear();
			foreach( string file in this.alCloseApplications )
			{
				this.lsbDownloadList.Items.Add( file );
			}
		}
		

		/// <summary>
		/// 控制文本控件
		/// </summary>
		/// <param name="bVisible"></param>
		private void ViewRichTextBox( bool bVisible,bool bCheckBox )
		{
			this.lsbDownloadList.Visible = false;
			this.prgBar.Visible = false;
			this.rchText.Visible = bVisible;
			this.m_StatusBar.Visible = false;
			if( bVisible )
			{
				this.rchText.Location = new Point( 0, 0 );
				this.rchText.Width = this.pnlContainer.Width - 2 * 6;
				if( bCheckBox )
				{
					this.rchText.Height = this.pnlContainer.Height - 2 * 6 - this.chkAgree.Height;
				}
				else
				{
					this.rchText.Height = this.pnlContainer.Height - 2 * 6;
				}
				if( this.m_StatusBar.Visible )
				{
					this.rchText.Height -= this.m_StatusBar.Height;
				}
			}
			this.ViewCheckBox( bCheckBox );
		}


		/// <summary>
		/// 控制ListBox控件
		/// </summary>
		/// <param name="bVisible"></param>
		/// <param name="ProgressBar"></param>
		private void ViewListBox( bool bVisible,bool bProgressBar )
		{
			this.rchText.Visible = false;
			this.chkAgree.Visible = false;			
			this.lsbDownloadList.Visible = bVisible;			
			if( bVisible )
			{
				this.lsbDownloadList.Location = new Point( 0, 6 );
				this.lsbDownloadList.Width = this.pnlContainer.Width - 2 * 6;
				this.lsbDownloadList.Height = this.pnlContainer.Height - 2 * 6;
				if( bProgressBar )
				{					
					this.lsbDownloadList.Height = this.pnlContainer.Height - 2 * 6 - this.prgBar.Height;
				}				
				if( this.m_StatusBar.Visible )
				{
					this.lsbDownloadList.Height = this.lsbDownloadList.Height - this.m_StatusBar.Height;
				}
			}
		}


		/// <summary>
		/// 控制ProgressBar控件
		/// </summary>
		/// <param name="bVisible"></param>
		private void ViewProgressBar( bool bVisible )
		{
			this.rchText.Visible = false;
			this.prgBar.Visible = bVisible;
			if( bVisible )
			{
				this.prgBar.Location = new Point( this.lsbDownloadList.Location.X,this.lsbDownloadList.Location.Y + this.lsbDownloadList.Height );
				this.prgBar.Width = this.pnlContainer.Width - 2 * 6;
			}
		}


		/// <summary>
		/// 控制CheckBox控件显示
		/// </summary>
		/// <param name="bVisible"></param>
		private void ViewCheckBox( bool bVisible )
		{
			this.lsbDownloadList.Visible = false;
			this.prgBar.Visible = false;
			this.chkAgree.Visible = bVisible;

			if( bVisible )
			{
				this.chkAgree.Location = new Point( this.rchText.Location.X,this.rchText.Bottom + 2 );
			}
		}
		

		/// <summary>
		/// 控制StatusBar控件显示
		/// </summary>
		/// <param name="bVisible"></param>
		private void ViewStatusBar( bool bVisible )
		{
			this.m_StatusBar.Visible = bVisible;			
		}


		#endregion		
	}
}
