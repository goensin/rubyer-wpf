# Rubyer-WPF

#### 介绍
**更新 2.0.0 版本，更加通用主题，通过部分参数配置可以改变整体主题样式，重新优化了控件样式，增加动画效果和添加部分控件附加功能；** <br/>
一款的 WPF 主题和控件包，免费开源，欢迎下载使用并点 ⭐，使用过程有问题反馈 issue 空闲会及时处理；接下来也会提供部分实用场景 Demo；<br/>
如果使用后感觉还不错，也可以提供一些捐赠作为作者持续更新的动力。

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
<Color x:Key="LightForegroundColor">#252526</Color>
<Color x:Key="LightBackgroundColor">#FFFFFF</Color>
<Color x:Key="DarkForegroundColor">#E6E6E6</Color>
<Color x:Key="DarkBackgroundColor">#1E1E1E</Color>
<SolidColorBrush x:Key="DefaultForeground" Color="{DynamicResource LightForegroundColor}" />
<SolidColorBrush x:Key="DefaultBackground" Color="{DynamicResource LightBackgroundColor}" />

<SolidColorBrush x:Key="WhiteForeground" Color="#FFFFFF" />
<SolidColorBrush x:Key="BlackForeground" Color="#000000" />
<SolidColorBrush x:Key="Primary" Color="#2196F3" />
<SolidColorBrush x:Key="Accent" Color="#F50057" />
<SolidColorBrush x:Key="Light" Color="#6EC6FF" />
<SolidColorBrush x:Key="Lighter" Color="#9BE7FF" />
<SolidColorBrush x:Key="Dark" Color="#0069C0" />
<SolidColorBrush x:Key="Darker" Color="#004BA0" />

<SolidColorBrush x:Key="Error" Color="#E63935" />
<SolidColorBrush x:Key="Info" Color="#909399" />
<SolidColorBrush x:Key="Warning" Color="#F57C00" />
<SolidColorBrush x:Key="Success" Color="#43A047" />
<SolidColorBrush x:Key="Question" Color="#2196F3" />

<SolidColorBrush x:Key="PrimaryText" Color="#212121" />
<SolidColorBrush x:Key="RegularText" Color="#484848" />
<SolidColorBrush x:Key="SeconarydText" Color="#6D6D6D" />
<SolidColorBrush x:Key="WatermarkText" Color="#AA6D6D6D" />

<SolidColorBrush x:Key="Border" Color="#9E9E9E" />
<SolidColorBrush x:Key="BorderLight" Color="#CFCFCF" />
<SolidColorBrush x:Key="BorderLighter" Color="#E0E0E0" />

<Color x:Key="EffectColor">#BDBDBD</Color>
<SolidColorBrush x:Key="Mask" Color="#9E9E9E" />
<SolidColorBrush x:Key="MaskDark" Color="#6D6D6D" />
<SolidColorBrush x:Key="DialogBackground" Color="#AA424242" />
<SolidColorBrush x:Key="HeaderBackground" Color="#CFCFCF" />
<SolidColorBrush x:Key="FunctionBarBackground" Color="#889E9E9E" />

```

部分控件含有中文文字说明，例如 PageBar 等，目前可支持切换至中英文;

```
 <ResourceDictionary Source="pack://application:,,,/Rubyer;component/Themes/Resources/I18N/en-US.xaml" />

```

可自定义控件和容器的圆角半径大小：

```
<!--  CornerRadius  -->
<CornerRadius x:Key="AllControlCornerRadius">3</CornerRadius>
<CornerRadius x:Key="LeftControlCornerRadius">3 0 0 3</CornerRadius>
<CornerRadius x:Key="RightControlCornerRadius">0 3 3 0</CornerRadius>
<CornerRadius x:Key="TopControlCornerRadius">3 3 0 0</CornerRadius>
<CornerRadius x:Key="BottomControlCornerRadius">0 0 3 3</CornerRadius>

<CornerRadius x:Key="AllContainerCornerRadius">5</CornerRadius>
<CornerRadius x:Key="LeftContainerCornerRadius">5 0 0 5</CornerRadius>
<CornerRadius x:Key="RightContainerCornerRadius">0 5 5 0</CornerRadius>
<CornerRadius x:Key="TopContainerCornerRadius">5 5 0 0</CornerRadius>
<CornerRadius x:Key="BottomContainerCornerRadius">0 0 5 5</CornerRadius>
```

#### Demo 截图

<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/Button.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/InputBox.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/SelectBox.png" height="400"/><br/>  
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/RangeBar.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/Icon.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/GroupBox.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/ListsTree.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/DataGrid.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/TabControl.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/DateTimeControl.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/MenuBar.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/TextBlock.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/PageBar.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/Message.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/MessageBox.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/Dialog.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/Transition.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/Badge.png" height="400"/><br/> 
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/Loading.png" height="400"/><br/> 