# Rubyer-WPF

#### 介绍
**更新 2.0.0 版本，更加通用主题，通过部分参数配置可以改变整体主题样式，重新优化了控件样式，增加动画效果和添加部分控件附加功能；** <br/>
一款的 WPF 主题和控件包，免费开源，欢迎下载使用并点 ⭐；<br/>
使用过程有问题反馈提交 issue 空闲会及时处理；<br/>
如果使用后感觉还不错，也可以提供一些捐赠作为作者持续更新的动力。

#### 交流讨论
欢迎提交 Issues

#### 软件架构
基于 .Net Framework 4.6 和 .Net Core 3.1 和 .Net 6 的 WPF 
 
#### 安装教程
Install-Package Rubyer 添加引用，
或者 Nuget 搜索 Rubyer 安装。

#### 使用说明

在 WPF 项目的 App.Xaml 中引用:

```
<Application.Resources>
      <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="pack://application:,,,/Rubyer;component/Themes/Generic.xaml" />
            </ResourceDictionary.MergedDictionaries>
      </ResourceDictionary>
</Application.Resources>
```
根据需要可自定义整体主题颜色：

```
    <SolidColorBrush x:Key="WhiteForeground" Color="#FFFFFF" />
    <SolidColorBrush x:Key="BlackForeground" Color="#000000" />

    <Color x:Key="LightDefaultForegroundColor">#252526</Color>
    <Color x:Key="DarkDefaultForegroundColor">#E6E6E6</Color>
    <SolidColorBrush x:Key="DefaultForeground" Color="{DynamicResource LightDefaultForegroundColor}" />

    <Color x:Key="LightDefaultBackgroundColor">#FFFFFF</Color>
    <Color x:Key="DarkDefaultBackgroundColor">#1E1E1E</Color>
    <SolidColorBrush x:Key="DefaultBackground" Color="{DynamicResource LightDefaultBackgroundColor}" />

    <Color x:Key="LightFloatBackgroundColor">#F8F8F8</Color>
    <Color x:Key="DarkFloatBackgroundColor">#303030</Color>
    <SolidColorBrush x:Key="FloatBackground" Color="{DynamicResource LightFloatBackgroundColor}" />

    <Color x:Key="LightControlBackgroundColor">#FFFFFF</Color>
    <Color x:Key="DarkControlBackgroundColor">#1E1E1E</Color>
    <SolidColorBrush x:Key="ControlBackground" Color="{DynamicResource LightControlBackgroundColor}" />

    <Color x:Key="LightContainerBackgroundColor">#FFFFFF</Color>
    <Color x:Key="DarkContainerBackgroundColor">#2D2D2D</Color>
    <SolidColorBrush x:Key="ContainerBackground" Color="{DynamicResource LightContainerBackgroundColor}" />

    <Color x:Key="LightPrimaryColor">#2196F3</Color>
    <Color x:Key="DarkPrimaryColor">#2196F3</Color>
    <SolidColorBrush x:Key="Primary" Color="{DynamicResource LightPrimaryColor}" />

    <Color x:Key="LightAccentColor">#F50057</Color>
    <Color x:Key="DarkAccentColor">#F50057</Color>
    <SolidColorBrush x:Key="Accent" Color="{DynamicResource LightAccentColor}" />

    <Color x:Key="LightLightColor">#6EC6FF</Color>
    <Color x:Key="DarkLightColor">#6EC6FF</Color>
    <SolidColorBrush x:Key="Light" Color="{DynamicResource LightLightColor}" />

    <Color x:Key="LightDarkColor">#0069C0</Color>
    <Color x:Key="DarkDarkColor">#0069C0</Color>
    <SolidColorBrush x:Key="Dark" Color="{DynamicResource LightDarkColor}" />

    <Color x:Key="LightBorderColor">#9E9E9E</Color>
    <Color x:Key="DarkBorderColor">#CFCFCF</Color>
    <SolidColorBrush x:Key="Border" Color="{DynamicResource LightBorderColor}" />

    <Color x:Key="LightBorderLightColor">#CFCFCF</Color>
    <Color x:Key="DarkBorderLightColor">#616161</Color>
    <SolidColorBrush x:Key="BorderLight" Color="{DynamicResource LightBorderLightColor}" />

    <Color x:Key="LightBorderLighterColor">#E0E0E0</Color>
    <Color x:Key="DarkBorderLighterColor">#424242</Color>
    <SolidColorBrush x:Key="BorderLighter" Color="{DynamicResource LightBorderLighterColor}" />

    <Color x:Key="LightSeconarydTextColor">#CC9E9E9E</Color>
    <Color x:Key="DarkSeconarydTextColor">#CCBDBDBD</Color>
    <SolidColorBrush x:Key="SeconarydText" Color="{DynamicResource LightSeconarydTextColor}" />

    <Color x:Key="LightWatermarkTextColor">#BB6D6D6D</Color>
    <Color x:Key="DarkWatermarkTextColor">#BBE0E0E0</Color>
    <SolidColorBrush x:Key="WatermarkText" Color="{DynamicResource LightWatermarkTextColor}" />

    <Color x:Key="LightEffectColor">#969696</Color>
    <Color x:Key="DarkEffectColor">#000000</Color>

    <Color x:Key="LightMaskColor">#9E9E9E</Color>
    <Color x:Key="DarkMaskColor">#9E9E9E</Color>
    <SolidColorBrush x:Key="Mask" Color="{DynamicResource LightMaskColor}" />

    <Color x:Key="LightMaskDarkColor">#6D6D6D</Color>
    <Color x:Key="DarkMaskDarkColor">#6D6D6D</Color>
    <SolidColorBrush x:Key="MaskDark" Color="{DynamicResource LightMaskDarkColor}" />

    <Color x:Key="LightDialogBackgroundColor">#99373737</Color>
    <Color x:Key="DarkDialogBackgroundColor">#88111111</Color>
    <SolidColorBrush x:Key="DialogBackground" Color="{DynamicResource LightDialogBackgroundColor}" />

    <Color x:Key="LightHeaderBackgroundColor">#1F000000</Color>
    <Color x:Key="DarkHeaderBackgroundColor">#1FFFFFFF</Color>
    <SolidColorBrush x:Key="HeaderBackground" Color="{DynamicResource LightHeaderBackgroundColor}" />

    <SolidColorBrush x:Key="Error" Color="#E63935" />
    <SolidColorBrush x:Key="Info" Color="#909399" />
    <SolidColorBrush x:Key="Warning" Color="#F57C00" />
    <SolidColorBrush x:Key="Success" Color="#43A047" />
    <SolidColorBrush x:Key="Question" Color="#2196F3" />

```

部分控件含有中文文字说明，例如 PageBar 等，目前可支持切换至中英文;

```
 <ResourceDictionary Source="pack://application:,,,/Rubyer;component/Themes/Resources/I18N/en-US.xaml" />

```
主题亮暗模式切换，默认跟随系统：

```
ThemeManager.SwitchThemeMode(ThemeMode.Black | ThemeMode.Light);

```
可自定义控件和容器的圆角半径大小：

```
ThemeManager.SwitchControlCornerRadius(控件圆角半径值);
ThemeManager.SwitchContainerCornerRadius(容器圆角半径值);

```

#### Demo 截图

<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/Button.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/InputBox.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/SelectBox.png" height="400"/><br/>  
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/RangeBar.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/Icon.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/GroupBox.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/Lists.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/DataGrid.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/TabControl.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/DateTime.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/MenuBar.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/TextBlock.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/PageBar.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/Message.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/MessageBox.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/Dialog.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/Transition.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/Badge.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/Loading.png" height="400"/><br/> 