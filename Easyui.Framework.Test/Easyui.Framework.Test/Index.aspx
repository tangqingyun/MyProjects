<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Easyui.Framework.Test.Index" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<title>Full Layout - jQuery EasyUI Demo</title>
<link rel="stylesheet" type="text/css" href="/themes/default/easyui.css"/>
<link rel="stylesheet" type="text/css" href="/themes/icon.css"/>
<link rel="stylesheet" type="text/css" href="/themes/demo.css"/>
<script type="text/javascript" src="script/jquery.min.js"></script>
<script type="text/javascript" src="script/jquery.easyui.min.js"></script>
    <style type="text/css">
        #easyui_left_tree li
        {
            height:20px;line-height:20px;
            margin-top:2px;
        }
    </style>
</head>
<body class="easyui-layout">
	<div data-options="region:'north',border:false" style="height:66px;background:url('images/head_bg1.jpg');padding:10px"></div>
	<div data-options="region:'west',split:true,title:'导航菜单'" style="width:13%;">
        <div id="accordions" class="easyui-accordion"  border="false" fit="true">  
             <div title="项目申请"  iconCls="tu1301">
                 <ul class="easyui-tree tree" id="easyui_left_tree">
                    <li iconCls="tu1304"><a id="" icon="tu1304" rel="/Apply.aspx">申请项目</a></li> 
                    <li iconCls="tu1305"><a id="A1" icon="tu1305" rel="/List.aspx">申请记录</a></li> 
                 </ul>
             </div>
             <div title="QA验证" iconCls="tu1302" ></div>
             <div title="项目部署" iconCls="tu1305" ></div>
        </div> 

	</div>
	<!--<div data-options="region:'east',split:true,collapsed:true,title:'East'" style="width:100px;padding:10px;">east region</div>-->
	<!--<div data-options="region:'south',border:false" style="height:50px;background:#A9FACD;padding:10px;">south region</div>-->
	<div data-options="region:'center',title:''">
        <div id="tabs" class="easyui-tabs" fit="true" border="false">  
            <%--<div title="我的工作台" iconCls="tu1306" style="padding:20px;"></div>
            <div title="申请列表" iconCls="tu1305" closable="true"></div>--%>
        </div>
         <iframe id="iframe" src="" frameborder="0" scrolling="auto" style="width: 100%; height:99%; -ms-overflow-y: auto;"></iframe>
	</div>
    <script type="text/javascript">

        $(function () {
            addTab("我的工作台", "/Welcome.aspx", "tu1306", false, "dd");
            $("#accordions ul li a").click(function () {
                var tabTitle = $(this).text();
                var url = $(this).attr("rel"); //获取地址
                var id = $(this).attr("id"); //获取id
                var icon = $(this).attr("icon"); //获取图标
                if (icon == "") {
                    icon = "icon-save";
                }
                addTab(tabTitle, url, icon, true, id);
            });

            ////获取是否为IE浏览器
            //var bro = navigator.userAgent;
            //var isIE2 = bro.indexOf("MSIE") > 0 ? 'IE' : 'others';
            //if (isIE2 == 'IE') {
            //    $('#help').show();
            //}
            //else {
            //    //右下角提醒
            //$.messager_han.show("<font color=red>Dpm</font>",
            //    '<a target="_blank" href="http://wpa.qq.com/msgrd?v=3&uin=984081106&site=qq&menu=yes"><img border="0" src="http://wpa.qq.com/pa?p=2:984081106:52" alt="Dpm出现问题请联系我，工位号199。--韩" title=""/>Dpm出现问题请联系我，工位号65。--韩</a>');
            //var msg = $("#msg").val();
            //$.messager_han.show("<font color=red>Dpm系统通知</font>",
            //  '<a target="_blank" href="www.baidu.com">' + msg + '</a>');
            //}

            loginout = function () {
                $.messager.confirm('系统提示', '您确定要退出本次登录吗?', function (r) {
                    if (r) {
                        location.href = '/Login/LogOut';
                    }
                });
            }

            addLink = function () {
                var id = $("#userid").val();
                $('#openXXXIframe')[0].src = '/PermissionManger/User/EditUser?id=' + id + '&editType="editPwd"';
                $('#adddiv').dialog({
                    width: 450,
                    height: 500,
                    title: "修改密码",
                    onClose: function () {
                        $('#usergridData').datagrid('reload');
                    }
                });
                $('#adddiv').dialog("open");
            }

            function showMyWindow(title, href, width, height, modal, minimizable, maximizable) {

                $('#myWindow').window({
                    title: title,
                    width: width === undefined ? 600 : width,
                    height: height === undefined ? 450 : height,
                    content: '<iframe scrolling="yes" frameborder="0"  src="' + href + '" style="width:100%;height:98%;"></iframe>',
                    //        href: href === undefined ? null : href, 
                    modal: modal === undefined ? true : modal,
                    minimizable: minimizable === undefined ? false : minimizable,
                    maximizable: maximizable === undefined ? false : maximizable,
                    shadow: false,
                    cache: false,
                    closed: false,
                    collapsible: false,
                    resizable: false,
                    loadingMessage: '正在加载数据，请稍等片刻......'
                });
            }

            function closeTab(closeTitle, newTab, newUrl, newIcon, newClosable, newId) {
                $('#tabs').tabs('close', closeTitle);
                //addTab(newTab, newUrl, newIcon, newCloseable, newId);
                if (!$('#tabs').tabs('exists', newTab)) {
                    $('#tabs').tabs('add', {
                        title: newTab,
                        content: createFrame(newUrl, newId),
                        closable: newClosable,
                        icon: newIcon
                    });
                } else {//如果存在，重新加载该Tab
                    $('#tabs').tabs('select', newTab);
                    var tab = $('#tabs').tabs('getSelected');
                    $('#tabs').tabs('update', {
                        tab: tab,
                        options: {
                            content: createFrame(newUrl, newId),
                            closable: newClosable
                        }
                    });
                }
            }

            function openTab(tabName, tablink, tabPic) {
                addTab(tabName, tablink, tabPic, true, "dd");
            }

            function openHelp() {
                addTab("系统帮助", "/Home/Help", "tu0912", true, "dd");
            }

        });
        

        function addTab(subtitle, url, icon, closable, id) {
            if (!$('#tabs').tabs('exists', subtitle)) {
                if (url != undefined && url != "") {
                    $('#tabs').tabs('add', {
                        title: subtitle,
                        content: createFrame(url, id),
                        closable: closable,
                        icon: icon
                    });
                }
            } else {
                $('#tabs').tabs('select', subtitle);
                var tab = $('#tabs').tabs('getSelected');
                if (url != undefined && url != "") {
                    $('#tabs').tabs('update', {
                        tab: tab,
                        options: {
                            content: createFrame(url, id),
                            closable: closable
                        }
                    });
                }
            }
        }

        function createFrame(url, id) {
            var s = '<iframe id="' + id + '" scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:99%;overflow-y: auto; "></iframe>';
            return s;
        }

        

    </script>
</body>
  
</html>
