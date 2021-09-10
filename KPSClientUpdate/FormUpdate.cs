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
	/// ��������UI����
	/// </summary>
	public class FormUpdate : System.Windows.Forms.Form
	{
		#region ϵͳ����

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
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// ���캯��
		/// </summary>
		/// <param name="obj">���¶���</param>
		/// <param name="appilcation">������ɺ�Ӧ������Ӧ�ó���</param>
		public FormUpdate( UpdateUtility obj )
		{
			//
			// Windows ���������֧���������
			//
			InitializeComponent();
			this.objUpdate = obj;
			//
			// TODO: �� InitializeComponent ���ú�����κι��캯������
			//
		}


		/// <summary>
		/// ������������ʹ�õ���Դ��
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

		#region Windows ������������ɵĴ���
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
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
            this.btnNext.Text = "��һ��";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(513, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "�ر�";
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
            this.chkAgree.Text = "ͬ����������";
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
            this.rchText.Text = "�ı���ʾ�ؼ�";
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
            this.Text = "������ܸ���";
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

		#region �ֶ�

		/// <summary>
		/// ����ʱ��رյĹ���Ӧ�ó����б�
		/// </summary>
		private ArrayList alCloseApplications;

		/// <summary>
		/// ���²���
		/// </summary>
		/// <remarks>
		///		1:��ʼ��������Ϣ
		///		2:��ʾЭ����Ϣ
		///		3:��ʾ������ʷ��¼��Ϣ
		///		4:���ظ����ļ�
		///		5:������Ϣ������Ϣ
		///		6:��ʾ���������Ϣ
		///		7:ִ���������˳����³���
		/// </remarks>
		private int nStep;

		/// <summary>
		/// ���ض���
		/// </summary>
		private UpdateUtility objUpdate;

		/// <summary>
		/// �����Ƿ�ɹ�
		/// </summary>
		private bool bSussful;

		#endregion

		#region UI�¼�

		/// <summary>
		/// ��ʼ������
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FormUpdate_Load(object sender, System.EventArgs e)  // ��������
		{
			this.bSussful = false;
            this.lblIntro.Text = "����[��һ��]���ظ���:";
            try
			{
				if( this.objUpdate.LocalUpdateUrlConfigInfo != null )
				{
					//this.Text = string.Format( "����[{0}]--������ܸ���",this.objUpdate.LocalUpdateUrlConfigInfo.strUpdateTitle );
					this.Text = string.Format("����ɨ���������", this.objUpdate.LocalUpdateUrlConfigInfo.strUpdateTitle);
				}				
				this.alCloseApplications = Global.ParseCloseApplications( this.objUpdate.RemoteConfigInfo );
				this.nStep = 4;  // �ӵ��Ĳ���ʼ
				//�����¼�
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
                this.ViewListBox(false, false);  // ��һ����������Ϊfalse
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
		/// ��һ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnNext_Click(object sender, System.EventArgs e)
		{	
			switch( this.nStep )
			{
                //case 2:
                //    this.StepTwo();  // ��ʾЭ��
                //    break;
                //case 3:
                //    this.StepThree();  // ��ʾ������ʷ��¼��Ϣ
                //    break;
                case 4:
					this.StepFour();  // ���ظ����ļ�
					break;
                case 5:
                    this.StepFive();  // ������Ϣ������Ϣ
                    break;
                case 6:
                    this.DisplayUpdateCompleteInfo();  // ��ʾ���������Ϣ
                    break;
                case 7:
					this.RunMainProgram();  // ����������
					break;					
			}
		}

		
		/// <summary>
		/// �ر�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnClose_Click(object sender, System.EventArgs e)
		{
			if( MessageBox.Show( "ȷ��Ҫֹͣ���ܸ�����", "ϵͳѯ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) == DialogResult.Yes )
			{
				Application.Exit();
			}
		}


		/// <summary>
		/// ������һ����ť״̬
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void chkAgree_CheckedChanged(object sender, System.EventArgs e)
		{
			this.btnNext.Enabled = this.chkAgree.Checked;
		}


		#endregion

		#region �����¼�

		/// <summary>
		/// ɾ����ʱ�ļ����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void objUpdate_DeleteTempFileComplete( object sender,EventArgs e )
		{
			this.rchText.Text += "\r\nɾ����ʱ�ļ����.";
		}


		/// <summary>
		/// ���ظ����ļ�����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void objUpdate_DownloadFileOver( object sender,EventArgs e )
		{
			this.lblIntro.Text = "���ظ����ļ����.";
			this.ViewStatusBar( false );
			this.ViewProgressBar( false );
			this.btnNext.Enabled = true;			
		}


		/// <summary>
		/// �������
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void objUpdate_UpdateComplete( object sender,EventArgs e )
		{	
			this.lblIntro.Text = "�������";			
			this.rchText.Text = string.Format( "Ӧ�ó���汾:{0}\r\n����������:{1:yyyy��MM��dd��}",this.objUpdate.RemoteConfigInfo.UpdateMainVersion,this.objUpdate.RemoteConfigInfo.UpdateDate );
			this.btnNext.Enabled = true;
			this.nStep = 6;
			this.bSussful = true;
		}


		/// <summary>
		/// ���±���������Ϣ���
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void objUpdate_UpdateLocalConfigInfoComplete( object sender,EventArgs e )
		{
			this.rchText.Text += "\r\n���±���������Ϣ���.\r\n����ɾ����ʱ�ļ�...";
		}


		/// <summary>
		/// ���±����ļ����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void objUpdate_UpdateLocalFilesComplete( object sender,EventArgs e )
		{
			this.rchText.Text += "\r\n���±����ļ����.\r\n���ڸ��±���������Ϣ...";						
		}


		/// <summary>
		/// ����Э����������¼�
		/// </summary>
		/// <param name="e"></param>
		private void objUpdate_DownloadLicenceInfoComplete( TextEventArgs e )
		{
			this.lblIntro.Text = "�û�Э������:";
			this.ViewRichTextBox( true,true );
			this.rchText.Text = e.Text;
//			this.btnNext.Enabled = true;
		}


		/// <summary>
		/// ���ظ��¼�¼��Ϣ����¼�
		/// </summary>
		/// <param name="e"></param>
		private void objUpdate_DownloadHistoryInfoComplete( TextEventArgs e )
		{
			this.lblIntro.Text = "������ʷ��¼:";
			this.ViewRichTextBox( true,false );
			this.rchText.Text = e.Text;
			this.btnNext.Enabled = true;
		}


		/// <summary>
		/// �����ļ����
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
		/// �����ļ������
		/// </summary>
		/// <param name="e"></param>
		private void objUpdate_DownloadFileBlockComplete( UpdateHelper.DownloadFileEventArgs e ) 
		{
			this.prgBar.Value = e.Downloads;
			this.sp_DownloadInfo.Text = string.Format( "{0}/{1}",e.Downloads,e.FileSize );
		}


		/// <summary>
		/// ��ʼ��������Ϣ����¼�
		/// </summary>
		/// <param name="e"></param>
		private void objUpdate_InializationDownloadInfoComplete( UpdateHelper.ArrayListEventArgs e )
		{
		}


		/// <summary>
		/// ׼�������ļ��¼�
		/// </summary>
		/// <param name="e"></param>
		private void objUpdate_PrepareDownloadFiles( UpdateHelper.ArrayListEventArgs e )
		{
			//��������ļ��б�
			this.lsbDownloadList.Items.Clear();
			foreach( FileDescription info in e.DataList )
			{
				this.lsbDownloadList.Items.Add( info );
			}
			Application.DoEvents();
		}


		/// <summary>
		/// ׼����ʼ�����ļ��¼�
		/// </summary>
		/// <param name="e"></param>
		private void objUpdate_InitlizationDownloadFile( UpdateHelper.DownloadFileInitEventArgs e )
		{
			//׼�������ļ�
            this.sp_DownFileName.Text = string.Format( "{0}[{1}KB]",e.FileName,Global.GetFileSizeOfKB( e.FileSize ) );
			this.prgBar.Maximum = e.FileSize;
		}


		#endregion

		#region ����

		/// <summary>
		/// ��ʾЭ����Ϣ
		/// </summary>
		//private void StepTwo()
		//{
		//	try
		//	{
		//		if( !this.CheckApplicationsIsClosed() )
		//		{
		//			MessageBox.Show( "���ȹرչ�����Ӧ�ó���","ϵͳ��ʾ" );
		//			return;
		//		}
		//		this.btnNext.Enabled = false;
		//		this.lblIntro.Text = "��������Э����Ϣ...";
		//		//��ʾЭ����Ϣ
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
						DialogResult answer = MessageBox.Show("���������е�ɨ��ˣ��Ƿ�رգ�", "ϵͳ��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
						if (answer == DialogResult.OK)
						{
							// ɱ������
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

				this.nStep = 4;  // ��һ�����
			}
			catch (Exception ex)
			{
				this.btnNext.Enabled = true;
				this.DisplayExceptionInfo(ex);
			}
		}


		/// <summary>
		/// ��ʾ������ʷ��¼��Ϣ
		/// </summary>
		private void StepThree()
		{
			try
			{				
				this.lblIntro.Text = "�������ظ�����ʷ��¼...";
				this.btnNext.Enabled = false;
				//��ʾЭ����Ϣ
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
		/// ���ظ����ļ�
		/// </summary>
		private void StepFour()
		{
			try
			{				
				this.lblIntro.Text = "�������ظ����ļ�...";
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
		/// ������Ϣ������Ϣ
		/// </summary>
		private void StepFive()
		{			
			this.ViewCheckBox( false );
			this.ViewListBox( false,false );
			this.ViewProgressBar( false );
			this.ViewRichTextBox( true,false );
			this.ViewStatusBar( false );
			this.lblIntro.Text = "���ڸ��±�����Ϣ,���Ժ�...";
			this.rchText.Text = "���ڸ��±����ļ�...";
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
		/// ��ʾ���������Ϣ
		/// </summary>
		private void DisplayUpdateCompleteInfo()
		{
			this.rchText.Text = string.Format( "\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n    �뵥��[���]���˳��򵼡�" );
			this.btnNext.Text = "���";
			this.nStep = 7;

			// ����Ҫ�ش�
			this.objUpdate.callback();
		}


		/// <summary>
		/// ��ʾ�쳣��Ϣ
		/// </summary>
		private void DisplayExceptionInfo( Exception ex )
		{
			this.lblIntro.Text = "����ʧ��:";
			this.btnNext.Text = "���";
			this.ViewRichTextBox( true,false );
			this.ViewStatusBar( false );
            System.Text.StringBuilder mString = new System.Text.StringBuilder();
			//mString.Append( string.Format( "����ʧ��!ԭ������:\r\n    {0}\r\n",ex.ToString() ) );
			//mString.Append( "    ����Զ�̷������Ĵ����¸��°��޷����أ��Դ��������Ǹ�⡣\r\n" );
			mString.Append("\r\n\r\n\r\n    ����Զ�̷������Ĵ����¸��°��޷����أ����Ժ����ԡ�\r\n");
			this.rchText.Text = mString.ToString();
			this.nStep = 7;
		}

         
		/// <summary>
		/// ����������
		/// </summary>
		private void RunMainProgram()
		{
			//���³ɹ�������������
			if( this.bSussful && this.objUpdate.LocalUpdateUrlConfigInfo.ApplicationFileName != null && this.objUpdate.LocalUpdateUrlConfigInfo.ApplicationFileName.Length > 0 )
			{
				System.Diagnostics.Process.Start( string.Format( "{0}\\{1}",Global.AssemblyPath,this.objUpdate.LocalUpdateUrlConfigInfo.ApplicationFileName ),this.objUpdate.LocalUpdateUrlConfigInfo.Parameters);
			}
			Application.Exit();
		}


		/// <summary>
		/// ������Ӧ�ó����Ƿ�ر�
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
		/// ��ʾӦ�رյ�Ӧ�ó����б�
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
		/// �����ı��ؼ�
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
		/// ����ListBox�ؼ�
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
		/// ����ProgressBar�ؼ�
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
		/// ����CheckBox�ؼ���ʾ
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
		/// ����StatusBar�ؼ���ʾ
		/// </summary>
		/// <param name="bVisible"></param>
		private void ViewStatusBar( bool bVisible )
		{
			this.m_StatusBar.Visible = bVisible;			
		}


		#endregion		
	}
}
