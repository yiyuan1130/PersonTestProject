using UnityEngine;
using System.Collections;
using System;  
using System.Runtime.InteropServices;  
using UnityEngine.UI;
 
public class popuxxx : MonoBehaviour {
 
    public Rect screenPosition;  
    [DllImport("user32.dll")]  
    static extern IntPtr SetWindowLong (IntPtr hwnd,int  _nIndex ,int  dwNewLong);  
    [DllImport("user32.dll")]  
    static extern bool SetWindowPos (IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);  
    [DllImport("user32.dll")]  
    static extern IntPtr GetForegroundWindow ();  
 
    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();
    [DllImport("user32.dll")]
    public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
 
    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);
 
    const uint SWP_SHOWWINDOW = 0x0040;  
    const int GWL_STYLE = -16;  
    const int WS_BORDER = 1;  
    const int WS_POPUP = 0x800000;  
    const int  SW_SHOWMINIMIZED   = 2; //{最小化, 激活}
    const int  SW_SHOWMAXIMIZED   = 3; //{最大化, 激活} 
    public void btn_onclick(){ //最小化 
        ShowWindow(GetForegroundWindow(), SW_SHOWMINIMIZED );
    }
    public void btn_onclickxx(){ //最大化
        ShowWindow(GetForegroundWindow(),SW_SHOWMAXIMIZED); 
    }
    IntPtr Handle;
    float xx;
    bool bx;
    void Start ()  
    {  
        bx = false;
        xx = 0f;
        #if UNITY_STANDALONE_WIN
        Resolution[] r = Screen.resolutions;
        screenPosition = new Rect ((r[r.Length-1].width-Screen.width)/2,(r[r.Length-1].height-Screen.height)/2,Screen.width,Screen.height);  
        SetWindowLong(GetForegroundWindow (), GWL_STYLE, WS_POPUP);//将网上的WS_BORDER替换成WS_POPUP  
        Handle = GetForegroundWindow ();   //FindWindow ((string)null, "popu_windows");
        SetWindowPos (GetForegroundWindow (), 0,(int)screenPosition.x,(int)screenPosition.y, (int)screenPosition.width,(int) screenPosition.height, SWP_SHOWWINDOW);  
        #endif 
    }  
 
 
    void Update(){
        #if UNITY_STANDALONE_WIN
        if (Input.GetMouseButtonDown (0)) { 
 
            xx =0f;
            bx=true;
        }
        // if(bx && xx>=0.3f ){ //这样做为了区分界面上面其它需要滑动的操作
        if(bx){ //这样做为了区分界面上面其它需要滑动的操作
            ReleaseCapture(); 
            SendMessage(Handle, 0xA1, 0x02, 0); 
            SendMessage(Handle, 0x0202, 0, 0);
 
 
        }
        if(bx)
            xx +=Time.deltaTime;
        if(Input.GetMouseButtonUp(0)){
 
            xx =0f;
            bx=false;
 
        }
 
        #endif 
    }
 
}