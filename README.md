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

* PSO2emergencyToDiscordCoreの終了
```Shell
> stop
```

* バージョンの表示
```Shell
> version
```

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
$ cd /var/pso2/PSO2emergencyToDiscordCore_1.0.0.0_linux-64/binary/
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