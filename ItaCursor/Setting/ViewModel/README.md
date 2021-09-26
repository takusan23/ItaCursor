# Setting.ViewModel 名前空間

ViewModelがあります。バインディングさせるとViewModelに置いた変数のsetter、getterが呼ばれるようになるそうですよ？

AndroidのときはViewModelを使うと画面回転を超えられる、Activityのコードが減らせるメリットがあるけどWPFの場合はどうなんだろう。

## ViewModelButtonClick
Viewに置いたボタンのクリックイベントをViewModelへ渡すときに使う。  

## ViewModelValueChanged
ViewModelで値を変更したときにViewに通知が行くようにしたもの。  
AndroidのLiveData的な？