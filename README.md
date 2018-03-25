# PSO2emergencyToDiscordCore
![screenshot](https://raw.githubusercontent.com/kousokujin/PSO2emergencyToDiscordCore/master/screenshot.png)

Windowsのみで動く[PSO2emergencyToDiscord](https://github.com/kousokujin/PSO2emergencyToDiscord)を . NET Core 2.0で作り直してWindowsだけではなくLinuxやmacOSで動くようにしたものです。

PSO2の予告緊急を1時間前、30分前にDiscordに通知するアプリケーションです。

DiscordのWebHooksをつかいます。

# 使い方
## 初期設定
通知したいDiscordのチャンネルでWebhooksを発行します。

PSO2emergencyToDiscordCoreを起動すると初期設定が行われるので、WebHooksのURLを入力します。

あとは30分前と1時間前にDiscordのチャンネルで通知が来ます。

## 起動方法
### Windows

ダウンロードしたzipファイルを解凍して、binaryフォルダ中のPSO2emergencyToDiscordCore.exeから起動。

Windows10(x64)で動作確認を行っています。

### Linux

ダウンロードしたzipファイルを解凍して、binaryフォルダ内にcdコマンドで入り、以下のコマンドを実行。
```Shell
$ ./PSO2emergencyToDiscordCore
```

Ubuntu 17.04(x64)で動作を確認しています。


### macOS

手元にmacOSのPCがないのでやり方は知らん。

## コマンド
* 任意の文字列をDiscordへの投稿
```Shell
> post [文字列]
```

* Discord WebHooks URLの再設定
```Shell
> url [WebHooksのURL]
```

* Discord WebHooks URLの確認
```Shell
> url
```

* デイリーオーダー「バル・ロドス討伐(VH)」の通知設定
```Shell
> rodos [enable|disable]
```

* デイリーオーダー「バル・ロドス討伐(VH)」の通知設定の確認
```Shell
> rodos
```

* 覇者の紋章キャンペーンの通知の有効化または無効化
```Shell
> hasha [enable|disable]
```

* 覇者の紋章キャンペーンの通知時間を記述したCSVファイルの読み込み
```Shell
> hasha csvfile [csvファイルの場所]
```

* 覇者の紋章キャンペーンの通知時間の確認
```Shell
> hasha list
```

* PSO2emergencyToDiscordCoreの終了
```Shell
> stop
```

* バージョンの表示
```Shell
> version
```

# 覇者の紋章キャンペーンの通知時間のCSVファイルの記述方法
通知したい曜日と時間(時、分、秒)をすべて以下のように記述する。
```Shell
曜日,時,分,秒
```

曜日は以下の表で対応する数字を記述する。
|日|月|火|水|木|金|土|毎日|
|:-:|:-:|:-:|:-:|:-:|:-:|:-:|:-:|
|0|1|2|3|4|5|6|7|

## 例
* 毎日0時0分0秒と12時0分0秒に通知
```Shell
7,0,0,0
7,12,0,0
```
* 毎日0時0分0秒と12時0分0秒と火曜日23時30分に通知
```Shell
7,0,0,0
7,12,0,0
2,23,30,0
```

覇者の紋章キャンペーンの通知のCSVファイルははhasha csvfileコマンドを使って読み込まれる。
# Dockerfileの使い方
* Dockerfileからイメージをビルド
```shell
$ docker build -t [任意のイメージ名] .
```

* イメージからコンテナを作成
```shell
$ docker run -itd [イメージ名]
```

* コンテナ内に入って初期設定
```shell
$ docker attach [コンテナID]
$ cd /var/pso2/PSO2emergencyToDiscordCore_1.0.0.1_linux-64/binary/
$ ./PSO2emergencyToDiscordCore
```

最後にCtrl+P Ctrl+Qの順でコンテナから抜ける。

# 予告緊急の取得など
https://github.com/aki-lua87/PSO2ema
を使ってます。

# ダウンロード
[ダウンロード](https://github.com/kousokujin/PSO2emergencyToDiscordCore/releases)

# ライセンス
Copyright (c) 2018 kousokujin.

Released under the [MIT license][].

[MIT license]:http://opensource.org/licenses/mit-license.php "MIT license"