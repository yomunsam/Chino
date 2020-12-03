
**Root Key** : appsettings.json -> `Chino` -> `Account`

| Item | ValueType | Desc | DefaultValue | Value Agreement |
|:-----|:----------|:-----|--------------|:----------------|
|`EnableRegister`|`bool`| Whether to open the function of registered account<br>是否开放注册账号相关功能 | `true` | `true`/`false` |
| `UserName:Register` | `bool` | Need to fill in the UserName when registering.<br>在注册的时候需要填写用户名| `true`|`true`/`false`|
| `UserName:RegisterRequire` |`bool`| The UserName must be filled in when registering.<br>注册时用户名是必填项| `true` | `true`/`false`|
| `UserName:Login`| `bool` | Can login using the UserName.<br>可以使用用户名来登录 | `true` | `true`/`false`|
| `Email:Register` | `bool` | Need to fill in the Email when registering.<br>在注册的时候需要填写邮箱| `true`|`true`/`false`|
| `Email:RegisterRequire` |`bool`| The Email must be filled in when registering.<br>注册时邮箱是必填项| `true` | `true`/`false`|
| `Email:Login`| `bool` | Can login using the Email.<br>可以使用邮箱来登录 | `true` | `true`/`false`|
