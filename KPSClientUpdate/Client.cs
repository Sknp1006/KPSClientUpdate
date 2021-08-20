using System;
using System.Windows.Forms;

using AutoUpdater.UpdateHelper;

namespace SmartUpdater
{
	/// <summary>
	/// ���ܸ��µ������
	/// </summary>
	class Updater
	{
		private static System.Threading.ManualResetEvent alDone;

		/// <summary>
		/// Ӧ�ó��������ڵ㡣
		/// </summary>		
		/// <param name="args">
		///		args[0]:�����ļ�����:ֻ�贫���ļ���
		/// </param>
		[STAThread]
		static void Main(string[] args)
		{	
			try
			{	
				string file = string.Format( "{0}\\Update.xml",Global.AssemblyPath );
				if( !System.IO.File.Exists( file ) )
				{
					MessageBox.Show( "�����ļ�������!" );
					return;
				}
				Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);  // 
				alDone = new System.Threading.ManualResetEvent( true );
				UpdateUtility obj = new UpdateUtility( file );
				obj.InializationDownloadInfoComplete += new ArrayListEventHandler(obj_InializationDownloadInfoComplete);
				alDone.Reset();

				//ִ�г�ʼ����Ϣ
				AutoUpdater.UpdateHelper.Global.WriteUpdateLog( string.Format( "{0}\t��ʼ��������Ϣ.",DateTime.Now.ToString( "F" ) ),false );
				obj.InitUpdateInfo();
				alDone.WaitOne();//�����߳�ֱ����ʼ�����
				
				if( obj.Enable )
				{     
					AutoUpdater.UpdateHelper.Global.WriteUpdateLog( string.Format( "{0}:����.",DateTime.Now ),true );
					//��Ҫ����,ִ�и��³���
					System.Windows.Forms.Application.Run( new AutoUpdater.UpdateHelper.FormUpdate( obj ) );
				}
				//else
				//{
				//	//�������,ִ��ָ����Ӧ�ó���
				//	AutoUpdater.UpdateHelper.Global.WriteUpdateLog( "ִ��Ӧ�ó���.",true );
				//	System.Diagnostics.Process.Start( string.Format( "{0}\\{1}",Global.AssemblyPath,obj.LocalUpdateUrlConfigInfo.ApplicationFileName ),obj.LocalUpdateUrlConfigInfo.Parameters );
				//}
			}
			catch( Exception ex )
			{
                #if DEBUG
				MessageBox.Show( ex.ToString() );
                #else
				MessageBox.Show( ex.Message );
                #endif
			}
		}

		private static void obj_InializationDownloadInfoComplete( ArrayListEventArgs e )
		{
			alDone.Set();
		}

		private static void Application_ThreadException( object sender,System.Threading.ThreadExceptionEventArgs e )
		{
			MessageBox.Show( e.Exception.Message );
		}
	}
}