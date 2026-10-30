# IMBlocker

《冰与火之舞》（A Dance of Fire and Ice，ADOFAI）输入法屏蔽模组。

用于避免中文 / 日文 / 韩文输入法的快捷键和候选窗影响游戏操作，并在聚焦输入框时自动恢复输入法。

## 功能

- **游玩时屏蔽输入法**：通过 Windows IMM32 上下文和 Unity `Input.imeCompositionMode` 禁用游戏窗口的输入法。
- **输入框自动恢复**：检测 `TMP_InputField` / `InputField` 的聚焦状态，编辑文本时自动恢复输入法。
- **异步输入修正**：当前 beta 的异步键盘路径删掉了 `LAlt` 特殊键，IMBlocker 会把 `LAlt` / `RAlt` 加回去，避免切换输入法时被判定为游戏输入。

## 设置

在模组管理器的设置页中（UMM：**Mods → IMBlocker → Settings**）：

- **启用 IMBlocker**：不重启游戏即可开关模组。
- **在非输入框时屏蔽输入法**：主功能开关。
- **语言**：English / 简体中文，或跟随游戏语言。

## 构建

在 `.env` 中配置游戏路径（参考 `.env.example`）：

```cmd
dotnet build -c Release
```

部署并启动指定加载器：

```cmd
dotnet build -c Release -p:Loader=UMM
```
