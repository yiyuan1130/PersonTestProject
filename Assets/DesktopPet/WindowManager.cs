        
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using UnityEngine;
using System.Diagnostics;
 
public class WindowManager : MonoBehaviour
{
    private static WindowManager instance;
 
    public static WindowManager Instance
    {
        get
        {
            return instance;
        }
    }
    // Use this for initialization
    [SerializeField]
    private Material m_Material;
    private struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }
    // Define function signatures to import from Windows APIs
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();
    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);
 
    public delegate bool WNDENUMPROC(IntPtr hwnd, uint lParam);
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, uint lParam);
    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr GetParent(IntPtr hWnd);
    [DllImport("user32.dll")]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, ref uint lpdwProcessId);
    [DllImport("kernel32.dll")]  
    public static extern void SetLastError(uint dwErrCode);
 
 
    // Definitions of window styles
    const int GWL_STYLE = -16;
    const uint WS_POPUP = 0x80000000;
    const uint WS_VISIBLE = 0x10000000;
    public const int width = 300;
    public const int height = 300;
    void Start()
    {
        SetWindowScreen(width, height);
        var margins = new MARGINS() { cxLeftWidth = -1 };
        var hwnd = GetProcessWnd();
        SetWindowLong(hwnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);
        DwmExtendFrameIntoClientArea(hwnd, ref margins);
 
    }
 
    void SetWindowScreen(int width, int height)
    {
        Screen.SetResolution(width, height, false);
    }
 
    void OnRenderImage(RenderTexture from, RenderTexture to)
    {
        Graphics.Blit(from, to, m_Material);
    }
 
    public static IntPtr GetProcessWnd()
    {
        IntPtr ptrWnd = IntPtr.Zero;
        uint pid = (uint)Process.GetCurrentProcess().Id;  // 当前进程 ID  
        bool bResult = EnumWindows(new WNDENUMPROC(delegate (IntPtr hwnd, uint lParam)
       {
           uint id = 0;
 
           if (GetParent(hwnd) == IntPtr.Zero)
           {
               GetWindowThreadProcessId(hwnd, ref id);
               if (id == lParam)    // 找到进程对应的主窗口句柄  
               {
                   ptrWnd = hwnd;   // 把句柄缓存起来  
                   SetLastError(0);    // 设置无错误  
                   return false;   // 返回 false 以终止枚举窗口  
               }
           }
 
           return true;
 
       }), pid);
        return (!bResult && Marshal.GetLastWin32Error() == 0) ? ptrWnd : IntPtr.Zero;
    }
 
}

      