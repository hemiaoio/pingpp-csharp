# Ping++ CSharp SDK 

## 简介

    此版本基于官方SDK克隆而来,只是简单的把WebRequest更改成了HttpClient. 
    参照官方Demo做了个支付请求的Demo并测试.

    其他功能的并未测试.

    欢迎使用issues提bug,或者提交合并请求.
## 版本要求
至少要求 .net core 1.1

## 注意

## 安装

```powershell
Install-Package PingPlusPlus.AspNetCore
```

## 接入方法
### 初始化
```c#
// 设置 API-KEY
Pingpp.Pingpp.SetApiKey("sk_test_ibbTe5jLGCi5rzfH4OqPW9KC");
```

### 设置请求签名密钥
密钥需要你自己生成，公钥请填写到 [Ping++ Dashboard](https://dashboard.pingxx.com)
```c#
Pingpp.Pingpp.SetPrivateKeyPath(@"你生成的私钥文件的路径");
```

### 支付
#### 发起支付请求
```c#
Charge ch = Charge.Create(Dictionary<String, Object> param);
```

#### 查询指定 charge 对象
```c#
Charge ch = Charge.Retrieve(String id);
```

#### 查询 charge 列表
```c#
ChargeList chs = Charge.List(Dictionary<String, Object> listParam);
```

### 退款
#### 发起 refund
```c#
Refund re = Refund.Create(String chId, Dictionary<String, Object> param);
```

#### 查询指定 refund
```c#
Refund re = Refund.Retrieve(String chId, String reId);
```

#### 查询 refund 列表
```c#
RefundList res = Refund.List(String chId, Dictionary<String, Object> listParam);
```

### 微信红包
#### 发送红包请求
```c#
RedEnvelope red = RedEnvelope.Create(Dictionary<String, Object> param);
```

#### 查询指定 RedEnvelope 对象
```c#
RedEnvelope red = RedEnvelope.Retrieve(String id);
```

#### 查询 RedEnvelope 列表
```c#
RedEnvelopeList reds = RedEnvelope.List(Dictionary<String, Object> listParam);
```

### 企业付款
#### 发送企业付款请求
```c#
Transfer tr = Transfer.Create(Dictionary<String, Object> param);
```

#### 查询指定 Transfer 对象
```c#
Transfer tr = Transfer.Retrieve(String id);
```

#### 查询 Transfer 列表
```c#
TransferList trs = Transfer.List(Dictionary<String, Object> listParam);
```

### Event
#### 查询指定 Event 对象
```c#
Event evt = Event.Retrieve(String id);
```

#### 查询 Event 列表
```c#
EventList evts = Event.List(Dictionary<String, Object> listParam);
```

详细信息请参考 [API 文档](https://pingxx.com/document/api)