using ItaCursor.WindowsAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;

namespace ItaCursor.WindowsAPITool
{
    class WindowsAPISetWindowsHookExTool
    {
        /// <summary>
        /// GC対策
        /// </summary>
        private WindowsAPISetWindowsHookEx.MouseHookProc mouseHookProc = null;

        /// <summary>
        /// Hook解除用
        /// </summary>
        private IntPtr hookId = IntPtr.Zero;

        /// <summary>
        /// トラックパッドようウィンドウのウィンドウハンドル
        /// </summary>
        private IntPtr trackPadWindowHandle = IntPtr.Zero;

        /// <summary>
        /// カーソルウィンドウのウィンドウハンドル
        /// </summary>
        private IntPtr virtualCursorWindowHandle = IntPtr.Zero;

        /// <summary>
        /// ドラック中ならtrue
        /// </summary>
        private bool isDragging = false;

        /// <summary>
        /// AddTouchRectの制御用
        /// </summary>
        private bool isClicked = false;

        /// <summary>
        /// スクロール操作中ならtrue
        /// </summary>
        private bool isScrolling = false;

        /// <summary>
        /// カーソル位置を入れておくやつ
        /// </summary>
        private WindowsAPICursor.POINT _currentCursorPos;

        /// <summary>
        /// 指の位置とカーソル位置がどれだけ離れているか
        /// </summary>
        private Point diffCursorTouchPos = new Point(0, 0);

        /// <summary>
        /// スクロール時に前回のスクロールとの差分を持っておくための
        /// </summary>
        private Point diffScrollTouchPos = new Point(0, 0);

        /// <summary>
        /// カーソル操作エリア
        /// </summary>
        private Rect trackPadAreaRect = new Rect(0, 0, 0, 0);

        /// <summary>
        /// スクロールバーエリア
        /// </summary>
        private Rect scrollAreaRect = new Rect(0, 0, 0, 0);

        /// <summary>
        /// SetWindowHookEx経由でタッチイベントがほしいときに使う。
        /// </summary>
        private Dictionary<Rect, Action> touchRectList = new();

        /// <summary>
        /// 無効にしたい場合はどうぞ。
        /// </summary>
        public bool IsEnable { get; set; } = true;

        /// <summary>
        /// 将来的にスクロール量を調整したいので
        /// </summary>
        public int ScrollValue { get; set; } = 1;

        /// <summary>
        /// スクロールする関数呼びすぎると固まるので
        /// </summary>
        private int scrollInvokeLimitCount = 0;


        /// <summary>
        /// コンストラクタ。アプリケーション終了時には「UnhookWindowsHookEx」を呼んでください。
        /// </summary>
        /// <param name="windowHandle">トラックパッドウィンドウのウィンドウハンドル</param>
        /// <param name="_TrackPadWindowHandle">カーソルウィンドウのウィンドウハンドル</param>
        public WindowsAPISetWindowsHookExTool(IntPtr _TrackPadWindowHandle, IntPtr _virtualCursorWindowHandle)
        {
            // マウスのイベントをWindows全体で監視。今回はこれでマウス操作を奪い、代わりにSetCursorPosを使い自前でカーソル移動をする。
            mouseHookProc = HookProc;
            trackPadWindowHandle = _TrackPadWindowHandle;
            virtualCursorWindowHandle = _virtualCursorWindowHandle;
            hookId = WindowsAPISetWindowsHookEx.SetWindowsHookEx(WindowsAPISetWindowsHookEx.WH_MOUSE_LL, mouseHookProc, WindowsAPISetWindowsHookEx.GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0);
        }

        /// <summary>
        /// カーソル操作エリアを設定する
        /// </summary>
        /// <param name="rect"></param>
        public void SetTrackPadRectArea(Rect rect) => trackPadAreaRect = rect;

        /// <summary>
        /// スクロールバーエリアを設定する
        /// </summary>
        /// <param name="rect"></param>
        public void SetScrollBarRectArea(Rect rect) => scrollAreaRect = rect;

        /// <summary>
        /// アプリケーション終了時に呼んでください。フックを解除します。
        /// </summary>
        public void UnhookWindowsHookEx() => WindowsAPISetWindowsHookEx.UnhookWindowsHookEx(hookId);


        /// <summary>
        /// このアプリの他ウィンドウ（子ウィンドウ）の四角形を返す。配列に入れないのはウィンドウが移動したとき対応できないので、カーソル移動時に呼ぶ
        /// </summary>
        /// <returns></returns>
        public List<Rect> GetWindowRectListFromCurrentApp()
        {
            var windowHandleList = Application.Current.Windows.Cast<Window>()
                .Select((window) => new WindowInteropHelper(window).Handle)
                .ToList();
            windowHandleList.RemoveAll((handle) => handle == trackPadWindowHandle || handle == virtualCursorWindowHandle);
            return windowHandleList.Select((handle) => WindowsAPIGetWindowRectTool.GetWindowRect(handle)).ToList();
        }

        /// <summary>
        /// SetWindowHookEx経由でタッチイベントを登録するやつを全部解除する
        /// </summary>
        public void RemoveAllTouchRect() => touchRectList.Clear();

        /// <summary>
        /// SetWindowHookExを通してクリックイベントを受け取る場合に使ってください。
        /// 
        /// SetTrackPadRectArea との違いは SetWindowHookEx を経由するか。
        /// 
        /// 経由した場合はカーソル位置が変わりませんが、経由しない場合はタッチ場所にカーソルが移動します。
        /// </summary>
        /// <param name="rect">範囲</param>
        /// <param name="action">押したときに呼ばれます。</param>
        public void AddTouchRect(Rect rect, Action action) => touchRectList.Add(rect, action);

        /// <summary>
        /// マウス操作時によばれる
        /// 
        /// タッチすると、WM_MOUSEMOVE -> WM_LBUTTONDOWN -> WM_MOUSEMOVE.... -> WM_LBUTTONUP
        /// 
        /// の順番で呼ばれる模様。WM_LBUTTONDOWNが一番最初ではない。
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0 || !IsEnable)
            {
                // 0未満のとき と 無効状態 は CallNextHookEx を呼ぶ
                return WindowsAPISetWindowsHookEx.CallNextHookEx(hookId, nCode, wParam, lParam);
            }

            // マウス（たっち）入力情報
            WindowsAPISetWindowsHookEx.MSLLHOOKSTRUCT mouseHookStruct = (WindowsAPISetWindowsHookEx.MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(WindowsAPISetWindowsHookEx.MSLLHOOKSTRUCT));

            var touchPosX = mouseHookStruct.pt.x;
            var touchPosY = mouseHookStruct.pt.y;
            // クリックがWindowsAPISendInputTool#SendClick()で行われたものかどうか
            var isClickFromAPICall = mouseHookStruct.dwExtraInfo == WindowsAPISendInputTool.MOUSE_CLICK_EXTRA_INFO;

            if (isClickFromAPICall)
            {
                // WindowsAPISendInputTool でクリックした場合も SetWindowsHookEx に来てしまうので対策
                return WindowsAPISetWindowsHookEx.CallNextHookEx(hookId, nCode, wParam, lParam);
            }

            // トラックパッドに触れているか
            var isTouchingTrackPad = trackPadAreaRect.Contains(touchPosX, touchPosY);
            // スクロールバーに触れているか
            var isTouchingScrollBar = scrollAreaRect.Contains(touchPosX, touchPosY);

            // トラックパッドに触れてなくて、操作中でもない場合は何もせずreturn
            if (!isTouchingTrackPad && !(isDragging || isScrolling))
            {
                // でもAddTouchRectクリック範囲内ならreturn許さない
                var containsRectList = touchRectList.Where((dic) => dic.Key.Contains(touchPosX, touchPosY));
                if (containsRectList.Count() == 0)
                {
                    return WindowsAPISetWindowsHookEx.CallNextHookEx(hookId, nCode, wParam, lParam);
                }
                else if (!isClicked)
                {
                    // クリック範囲内なので押す
                    foreach (var containsRect in containsRectList)
                    {
                        containsRect.Value.Invoke();
                    }
                    isClicked = true;
                }
            }

            switch (wParam.ToInt32())
            {
                case WindowsAPISetWindowsHookEx.WM_MOUSEMOVE:
                    // スクロールバー
                    if (isTouchingScrollBar && !isScrolling)
                    {
                        // 一番最初 タッチ位置を保存しておく
                        diffScrollTouchPos = new Point(touchPosX, touchPosY);
                        isScrolling = true;
                    }
                    // 移動中。差分を見る
                    if (isScrolling)
                    {
                        var draggingScrollDiffY = touchPosY - (int)diffScrollTouchPos.Y;
                        // 0は動きないので
                        if (draggingScrollDiffY != 0)
                        {
                            scrollInvokeLimitCount++;
                            // 上にスクロールするのか下にスクロールするのか
                            var scroll = draggingScrollDiffY > 0 ? ScrollValue : -ScrollValue;
                            // SendScroll（内部でSendInput）とそれを呼ぶスレッドを呼びすぎないようにする
                            if (scrollInvokeLimitCount == 10)
                            {
                                new Thread(() => { WindowsAPISendInputTool.SendScroll(scroll); }).Start();
                                scrollInvokeLimitCount = 0;
                            }
                        }
                        diffScrollTouchPos = new Point(touchPosX, touchPosY);
                    }

                    // スクロール時以外はカーソル動かす
                    if (!isScrolling)
                    {
                        if (!isDragging)
                        {
                            // 一番最初。マウスポインタとの距離を保存しておく
                            WindowsAPICursor.GetCursorPos(out _currentCursorPos);
                            diffCursorTouchPos = new Point(_currentCursorPos.X - touchPosX, _currentCursorPos.Y - touchPosY);
                            isDragging = true;
                        }
                        if (isDragging)
                        {
                            // 移動中。差分を足す
                            var draggingMousePointerPosX = touchPosX + (int)diffCursorTouchPos.X;
                            var draggingMousePointerPosY = touchPosY + (int)diffCursorTouchPos.Y;
                            WindowsAPICursor.SetCursorPos(draggingMousePointerPosX, draggingMousePointerPosY);
                            // 直接SetCursorPosで指定した値を入れてもいいんだけど、画面外の対応が面倒なので取得する
                            WindowsAPICursor.GetCursorPos(out _currentCursorPos);
                            WindowsAPISetWindowPosTool.SetWindowPos(virtualCursorWindowHandle, _currentCursorPos.X, _currentCursorPos.Y);
                        }
                    }
                    break;
                case WindowsAPISetWindowsHookEx.WM_LBUTTONDOWN:
                    Debug.WriteLine("WM_LBUTTONDOWN");
                    // WM_MOUSEMOVE に移管
                    break;
                case WindowsAPISetWindowsHookEx.WM_LBUTTONUP:
                    Debug.WriteLine("WM_LBUTTONUP");
                    // 離したとき
                    isDragging = false;
                    isClicked = false;
                    isScrolling = false;
                    Debug.WriteLine("はなした");
                    break;
                default:
                    break;
            }

            // SetCursorPos関数で移動させるため無視
            return new IntPtr(1);
        }
    }
}
