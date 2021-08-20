using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Reflection;
using System.Web;
using System.Xml;

namespace AutoUpdater.UpdateHelper
{	
	/// <summary>
	/// ���������¼�ί�ж���
	/// </summary>
	public delegate void DownloadFileEventHandler( DownloadFileEventArgs e );

	/// <summary>
	/// ��ʼ�����ļ�ί�ж���
	/// </summary>
	public delegate void DownloadFileStartEventHandler( DownloadFileStartEventArgs e );

	/// <summary>
	/// �����ļ�
	/// </summary>
	public class DownloadFile : System.IDisposable
	{
		#region �¼�

		/// <summary>
		/// �����ļ���ʼ�¼�����
		/// </summary>
		public event DownloadFileStartEventHandler DownloadFileStart;

		/// <summary>
		/// �����ļ��¼�����
		/// </summary>
		public event DownloadFileEventHandler DownloadFileBlock;

		/// <summary>
		/// �����ļ�����¼�����
		/// </summary>
		public event EventHandler DownloadFileComplete;

		#endregion

		#region �ֶ�

		/// <summary>
		/// �����ص�URL
		/// </summary>
		private string strUrl;

		/// <summary>
		/// ������ɺ󱣴���ļ�����
		/// </summary>
		private string strFile;

		/// <summary>
		/// �ļ���С
		/// </summary>
		private int nFileSize;

		/// <summary>
		/// �Ƿ����ļ���С
		/// </summary>
		private bool bCheckFileSize;

		/// <summary>
		/// web�������
		/// </summary>
		private System.Net.HttpWebRequest objWebRequest;

		/// <summary>
		/// web���ն���
		/// </summary>
		private System.Net.HttpWebResponse objWebResponse;

		#endregion

		#region ���캯��

		/// <summary>
		/// Ĭ�Ϲ��캯��
		/// </summary>
		public DownloadFile()
		{
			this.nFileSize = 0;
			this.bCheckFileSize = false;
			this.strFile = string.Empty;
			this.strUrl = string.Empty;
		}


		#endregion

		#region ����

		/// <summary>
		/// ���ص�ַ
		/// </summary>
		public string DownloadUrl
		{
			get{ return this.strUrl; }
			set{ this.strUrl = value; }
		}


		/// <summary>
		/// �����ļ�
		/// </summary>
		public string DownloadFileName
		{
			get{ return this.strFile; }
			set{ this.strFile = value; }
		}


		/// <summary>
		/// �ļ���С
		/// </summary>
		public int FileSize
		{
			get{ return this.nFileSize; }
			set{ this.nFileSize = value; }
		}


		/// <summary>
		/// �Ƿ����ļ���С
		/// </summary>
		public bool CheckFileSize
		{
			get{ return this.bCheckFileSize; }
			set{ this.bCheckFileSize = value; }
		}

		#endregion

		#region ����

		/// <summary>
		/// �����ļ�
		/// </summary>
		public void Download()
		{
			FileStream fs = new FileStream( this.strFile,FileMode.Create,FileAccess.Write,FileShare.ReadWrite );
			try
			{
				this.objWebRequest = (HttpWebRequest)WebRequest.Create( this.strUrl );
				this.objWebRequest.Headers.Add("Accept-Encoding", "identity");  // 
				this.objWebRequest.AllowAutoRedirect = true;
//				int nOffset = 0;
				long nCount = 0;
				byte[] buffer = new byte[ 1024 * 1024 ];	//1MB
				int nRecv = 0;	//���յ����ֽ���
				this.objWebResponse = (HttpWebResponse)this.objWebRequest.GetResponse();
				Stream recvStream = this.objWebResponse.GetResponseStream();
				long nMaxLength = (int)this.objWebResponse.ContentLength;
				if (nMaxLength == -1)
                {
					nMaxLength = this.nFileSize;

				}
                else
                {
					if (this.bCheckFileSize && nMaxLength != this.nFileSize)
					{
						throw new Exception(string.Format("�ļ�\"{0}\"����,�޷�����!", Path.GetFileName(this.strFile)));
					}
				}

                if ( this.DownloadFileStart != null )
					this.DownloadFileStart( new DownloadFileStartEventArgs( (int)nMaxLength ) );
				while( true )
				{
					nRecv = recvStream.Read( buffer,0,buffer.Length );
					if( nRecv == 0 )
						break;
					fs.Write( buffer,0,nRecv );
					nCount += nRecv;
					//�������ؿ�����¼�
					if( this.DownloadFileBlock != null )
						this.DownloadFileBlock( new DownloadFileEventArgs( (int)nMaxLength,(int)nCount ) );
				}
				recvStream.Close();	
				//������������¼�
				if( this.DownloadFileComplete != null )
					this.DownloadFileComplete( this,EventArgs.Empty );
			}
			finally
			{
				fs.Close();
//				int nTmp = (int)(new FileInfo( this.strFile ).Length );
//				if( this.nFileSize != nTmp )
//				{
//					//File.Delete( this.strFile );
//					System.Windows.Forms.MessageBox.Show( string.Format( "{0}��С����ȷ.{1},{2}",this.strFile,this.nFileSize,nTmp ) );
//					throw new Exception( string.Format( "�ļ�\"{0}\"����,�޷�����!",Path.GetFileName( this.strFile ) ) );
//				}
//
			}
		}


		#endregion

		#region IDisposable ��Ա

		public void Dispose()
		{
			if( this.objWebRequest != null )
				this.objWebRequest = null;
			if( this.objWebResponse != null )
				this.objWebResponse.Close();
		}

		#endregion
	}

	/// <summary>
	/// ��ʼ�����ļ��¼�
	/// </summary>
	public class DownloadFileStartEventArgs : System.EventArgs 
	{

		#region �¼�����

		private int nFileSize;

		public DownloadFileStartEventArgs( int filesize )
		{
			this.nFileSize = filesize;
		}


		/// <summary>
		/// �ļ�����
		/// </summary>
		public int FileSize
		{
			get{ return this.nFileSize; }
		}

		#endregion
	}

	/// <summary>
	/// �����ļ������¼�
	/// </summary>
	public class DownloadFileEventArgs : System.EventArgs 
	{
		#region �¼�����

		/// <summary>
		/// �ļ���С
		/// </summary>
		private int nFileSize;

		/// <summary>
		/// ��ǰ��������
		/// </summary>
		private int nDownloads;

		/// <summary>
		/// ���캯��
		/// </summary>
		/// <param name="filesize">�ļ���С</param>
		/// <param name="downloads">��ǰ��������</param>
		public DownloadFileEventArgs( int filesize,int downloads )
		{
			this.nFileSize = filesize;
			this.nDownloads = downloads;
		}


		/// <summary>
		/// �ļ���С
		/// </summary>
		public int FileSize
		{
			get{ return this.nFileSize; }
			set{ this.nFileSize = value; }
		}


		/// <summary>
		/// ��ǰ��������
		/// </summary>
		public int Downloads
		{
			get{ return this.nDownloads; }
			set{ this.nDownloads = value; }
		}

		#endregion
	}
}
