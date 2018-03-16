# PSO2emergencyToDiscordCore

Windowsのみで動く[PSO2emergencyToDiscord](https://github.com/kousokujin/PSO2emergencyToDiscord)を . NET Core 2.0で作り直してWindowsだけではなくLinuxやmacOSで動くようにしたものです。

PSO2の予告緊急を1時間前、30分前にDiscordに通知するアプリケーションです。

DiscordのWebHooksをつかいます。

# 使い方
## 初期設定
通知したいDiscordのチャンネルでWebhooksを発行します。

PSO2emergencyToDiscordCoreを起動すると初期設定が行われるので、WebHooksのURLを入力します。

あとは30分前と1時間前にDiscordのチャンネルで通知が来ます。

## コマンド
* 任意の文字列をDiscordへの投稿
```Shell
post [文字列]
```

* Discord WebHooks URLの再設定
```Shell
url [WebHooksのURL]
```

* Discord WebHooks URLの確認
```Shell
url
```

* デイリーオーダー「バル・ロドス討伐(VH)」の通知設定
```Shell
rodos [enable|disable]
```

* デイリーオーダー「バル・ロドス討伐(VH)」の通知設定の確認
```Shell
rodos
```

* PSO2emergencyToDiscordCoreの終了
```Shell
stop
```

* バージョンの表示
```Shell
version
```
# 予告緊急の取得など
https://github.com/aki-lua87/PSO2ema
を使ってます。

# ダウンロード
[ダウンロード]()

## License
MIT