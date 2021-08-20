using System;
using System.Collections;

namespace AutoUpdater.UpdateHelper
{
	#region �¼�ί�ж�������

	/// <summary>
	/// �������ݼ����¼�����ί�ж���
	/// </summary>
	public delegate void ArrayListEventHandler( ArrayListEventArgs e );

	/// <summary>
	/// �����ı��¼�����ί�ж���
	/// </summary>
	public delegate void TextEventHandler(  TextEventArgs e );

	/// <summary>
	/// �������ļ��¼�����ί�ж���
	/// </summary>
	public delegate void DownloadFileInitEventHandler( DownloadFileInitEventArgs e );

	#endregion 

	/// <summary>
	/// ׼�������ļ��¼�����
	/// </summary>
	public class DownloadFileInitEventArgs : System.EventArgs 
	{
		#region �¼�����

		/// <summary>
		/// ׼�����ص��ļ�����
		/// </summary>
		private string strFile;

		/// <summary>
		/// ׼�������ļ���С
		/// </summary>
		private int nFileSize;

		public DownloadFileInitEventArgs( string file,int size )
		{
			this.strFile = file;
			this.nFileSize = size;
		}


		/// <summary>
		/// ׼�����ص��ļ�����
		/// </summary>
		public string FileName
		{
			get{ return this.strFile; }			
		}


		/// <summary>
		/// ׼�������ļ���С
		/// </summary>
		public int FileSize
		{
			get{ return this.nFileSize; }
		}


		#endregion
	}

	/// <summary>
	/// �������ݼ����¼�������
	/// </summary>
	public class ArrayListEventArgs : System.EventArgs 
	{
		#region �¼�����

		/// <summary>
		/// ���ݼ���
		/// </summary>
		private ArrayList alList;

		public ArrayListEventArgs( ArrayList al )
		{
			this.alList = al;
		}


		/// <summary>
		/// �¼�����
		/// </summary>
		public ArrayList DataList
		{
			get{ return this.alList; }
		}

		#endregion
	}


	/// <summary>
	/// �����ı��¼�����
	/// </summary>
	public class TextEventArgs : System.EventArgs 
	{
		#region �¼�����

		/// <summary>
		/// �ı�����
		/// </summary>
		private string strText;

		public TextEventArgs( string text )
		{
			this.strText = text;
		}


		/// <summary>
		/// �ı�����
		/// </summary>
		public string Text
		{
			get{ return this.strText; }
		}

		#endregion
	}
}
