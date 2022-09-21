# Unity3D-Editor-UI-Toolkit-Example
平时写过的Editor扩展工具积累。

## 1.一个简单的物品编辑工具demo
![image](https://user-images.githubusercontent.com/25300766/190916505-ab5658dd-b70b-49c5-a663-0507f0aa94b1.png)

> 主要代码：ItemEditor.cs，ItemEditor.uxml是手工搭出来的。
>
> 参考资料：麦田物语教程No.13: <https://learn.u3d.cn/tutorial/MFarmCourse/?tab=materials#62179c4dec04af00209be146>
>
> 注意，可能在点击MyTools->ItemEditor之后没反应，这是因为Editor ToolKit的脚本出错了可能只报一次错或者不报错，出现错误之后就不再编译运行，非常的坑。**调试时可以改变类名**，使其重新编译一次。

## 2.使选中的物体颜色改变
![image](https://user-images.githubusercontent.com/25300766/191518894-ab8b2974-ec6f-4246-ba6e-cc6e29e3fbf4.png)
> 主要代码：ExampleWindow.cs
>
> 参考资料：https://www.youtube.com/watch?v=491TSNwXTIg&t=204s
