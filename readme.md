## Guide

这是一个帮助提高听写效果的工具，你可以记录孩子的听写记录，筛选出最容易出错的词语，单词，以便于强化听写效果。

1. 登记宝宝 -> **选择一个宝宝**用于稍后的记录和分析；
2. 新建教材 -> 导入教材字库 -> 为刚刚选择的宝宝**选择一个教材**用于稍后的记录和分析；
3. 查询字词 (基于刚刚选择的宝宝和教材字库) -> **添加听写记录**(或者批量导入)；
4. 查询字词 -> 按照错误频率 或者 听写次数过滤等(根据上一步产生的听写记录) -> **筛选和反复听写易错字词**。

## 技术栈

dotnet 8.0 (C# 12) + MAUI Blazor cross platform framework， database is SQLite (support AzureSQL/SQL Server).

## Package

### Publish to Windows
[publish a MSIX package](https://learn.microsoft.com/en-us/dotnet/maui/windows/deployment/publish-cli?view=net-maui-8.0)
[publish a unpacked package](https://learn.microsoft.com/en-us/dotnet/maui/windows/deployment/publish-unpackaged-cli?view=net-maui-8.0)

```bash
dotnet publish -f net8.0-windows10.0.19041.0 -c Release -p:RuntimeIdentifierOverride=win10-x64 --self-contained

```

### Publish to Android

```powershell
keytool -genkeypair -v -keystore cleanCourse.keystore -alias cleanCourseKey -keyalg RSA -keysize 2048 -validity 10000
keytool -list -keystore cleanCourse.keystore

AndroidSigningPassword=PwdOfTheKey
dotnet publish -f net8.0-android -c Release -p:AndroidKeyStore=true -p:AndroidSigningKeyStore=cleanCourse.keystore -p:AndroidSigningKeyAlias=cleanCourseKey -p:AndroidSigningKeyPass=env:AndroidSigningPassword -p:AndroidSigningStorePass=env:AndroidSigningPassword
```

```bash
dotnet publish -f net8.0-android -c Release -p:AndroidKeyStore=true -p:AndroidSigningKeyStore=cleanCourse.keystore -p:AndroidSigningKeyAlias=cleanCourseKey -p:AndroidSigningKeyPass=$AndroidSigningPassword -p:AndroidSigningStorePass=$AndroidSigningPassword

```

### Install

#### Windows

After published, install by running: CleanCourse_[version]_[arch].msix

### Android

WIP