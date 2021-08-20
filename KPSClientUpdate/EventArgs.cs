using System;
using System.Collections;

namespace AutoUpdater.UpdateHelper
{
	#region 事件委托对象声明

	/// <summary>
	/// 传递数据集合事件数据委托对象
	/// </summary>
	public delegate void ArrayListEventHandler( ArrayListEventArgs e );

	/// <summary>
	/// 传递文本事件数据委托对象
	/// </summary>
	public delegate void TextEventHandler(  TextEventArgs e );

	/// <summary>
	/// 备下载文件事件数据委托对象
	/// </summary>
	public delegate void DownloadFileInitEventHandler( DownloadFileInitEventArgs e );

	#endregion 

	/// <summary>
	/// 准备下载文件事件数据
	/// </summary>
	public class DownloadFileInitEventArgs : System.EventArgs 
	{
		#region 事件数据

		/// <summary>
		/// 准备下载的文件名称
		/// </summary>
		private string strFile;

		/// <summary>
		/// 准备下载文件大小
		/// </summary>
		private int nFileSize;

		public DownloadFileInitEventArgs( string file,int size )
		{
			this.strFile = file;
			this.nFileSize = size;
		}


		/// <summary>
		/// 准备下载的文件名称
		/// </summary>
		public string FileName
		{
			get{ return this.strFile; }			
		}


		/// <summary>
		/// 准备下载文件大小
		/// </summary>
		public int FileSize
		{
			get{ return this.nFileSize; }
		}


		#endregion
	}

	/// <summary>
	/// 传递数据集合事件数据类
	/// </summary>
	public class ArrayListEventArgs : System.EventArgs 
	{
		#region 事件数据

		/// <summary>
		/// 数据集合
		/// </summary>
		private ArrayList alList;

		public ArrayListEventArgs( ArrayList al )
		{
			this.alList = al;
		}


		/// <summary>
		/// 事件数据
		/// </summary>
		public ArrayList DataList
		{
			get{ return this.alList; }
		}

		#endregion
	}


	/// <summary>
	/// 传递文本事件数据
	/// </summary>
	public class TextEventArgs : System.EventArgs 
	{
		#region 事件数据

		/// <summary>
		/// 文本内容
		/// </summary>
		private string strText;

		public TextEventArgs( string text )
		{
			this.strText = text;
		}


		/// <summary>
		/// 文本内容
		/// </summary>
		public string Text
		{
			get{ return this.strText; }
		}

		#endregion
	}
}
