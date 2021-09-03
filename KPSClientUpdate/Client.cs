using System;
using System.Windows.Forms;

using AutoUpdater.UpdateHelper;

namespace SmartUpdater
{
	/// <summary>
	/// 智能更新调用入口
	/// </summary>
	class Updater
	{
		private static System.Threading.ManualResetEvent alDone;

		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>		
		/// <param name="args">
		///		args[0]:配置文件名称:只需传递文件名
		/// </param>
		[STAThread]
		static void Main(string[] args)
		{	
			try
			{	
				string file = string.Format( "{0}\\Update.xml",Global.AssemblyPath );
				if( !System.IO.File.Exists( file ) )
				{
					MessageBox.Show( "配置文件不存在!" );
					return;
				}
				Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);  // 
				alDone = new System.Threading.ManualResetEvent( true );
				UpdateUtility obj = new UpdateUtility( file );
				obj.InializationDownloadInfoComplete += new ArrayListEventHandler(obj_InializationDownloadInfoComplete);
				alDone.Reset();

				//执行初始化信息
				AutoUpdater.UpdateHelper.Global.WriteUpdateLog( string.Format( "{0}\t初始化下载信息.",DateTime.Now.ToString( "F" ) ),false );
				obj.InitUpdateInfo();
				alDone.WaitOne();//阻塞线程直到初始化完成
				
				if( obj.Enable )
				{
					DialogResult answer;
					answer = MessageBox.Show("扫描端有新版本，请更新！", "鲲鹏扫描端升级程序", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
					if (answer == DialogResult.OK)
                    {
						AutoUpdater.UpdateHelper.Global.WriteUpdateLog(string.Format("{0}:更新.", DateTime.Now), true);
						//需要更新,执行更新程序
						System.Windows.Forms.Application.Run(new AutoUpdater.UpdateHelper.FormUpdate(obj));
					}
					else if (answer == DialogResult.Cancel)
                    {
						// 本次更新取消
						return;
                    }
				}
				//else
				//{
				//	//不需更新,执行指定的应用程序
				//	AutoUpdater.UpdateHelper.Global.WriteUpdateLog( "执行应用程序.",true );
				//	System.Diagnostics.Process.Start( string.Format( "{0}\\{1}",Global.AssemblyPath,obj.LocalUpdateUrlConfigInfo.ApplicationFileName ),obj.LocalUpdateUrlConfigInfo.Parameters );
				//}
			}
			catch( Exception ex )
			{
    //            #if DEBUG
				//MessageBox.Show( ex.ToString() );
    //            #else
				//MessageBox.Show( ex.Message );
    //            #endif
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
