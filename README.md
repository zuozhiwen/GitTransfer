# GitTransfer
多仓库代码同步

该工具适用于这样的场景:比如同一份代码要维护到两套不同的仓库，两个仓库git记录不是同一套，通过git是没有办法merge的，这时候可以用该工具来进行代码同步。

使用方法： 
```
gitTransfer --since "(2021/4/1 | 2021/4/1 12:00)" SrcSourceCodePath DstSourceCodePath
```
