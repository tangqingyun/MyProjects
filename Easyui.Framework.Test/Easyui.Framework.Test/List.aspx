<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Easyui.Framework.Test.List" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" type="text/css" href="/themes/default/easyui.css"/>
<link rel="stylesheet" type="text/css" href="/themes/icon.css"/>
<link rel="stylesheet" type="text/css" href="/themes/demo.css"/>
<script type="text/javascript" src="script/jquery.min.js"></script>
<script type="text/javascript" src="script/jquery.easyui.min.js"></script>
    <style type="text/css">
        html, body
        {
            padding:5px;
        }
    </style>
</head>
<body>

<table id="toolBar" width="100%">
    <tr>
        <td colspan="13">
            <a href="#" onclick="addLink()" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:'true'">
                添加申请</a>
            <a href="#" onclick="ToolBarShowApply()" class="easyui-linkbutton" data-options="iconCls:'icon-ok',plain:'true'">
                查看项目</a>
            <a href="#" onclick="ToolBarEditApply()" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:'true'">
                修改项目</a>
            <a href="#" onclick="ToolBarDeleteApply()" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:'true'">
                作废项目</a>
        </td>
        </tr>
    <tr>
    <td style="text-align: right;">项目名称关键字：</td>
    <td>
        <input type="text" style="width: 92px;" id="projectSrhName" name="projectSrhName" class="pagination-num" value="" />
    </td>
    <td>项目编号：</td>
    <td>
        <input type="text" style="width: 93px;" id="projectSrhId" name="projectSrhId" class="easyui-numberbox pagination-num" value="" data-options="min:1000,max:9999" />
    </td>
    <td style="text-align: right; width: 100px;" id="tdStartDate">开始日期：
    </td>
    <td>
        <input id="startDate" name="startDate" class="easyui-datebox" style="width: 100px">
    </td>
    <td style="text-align: right;" id="tdEndDate">结束日期：
    </td>
    <td>
        <input id="endDate" name="endDate" class="easyui-datebox" style="width: 100px">
    </td>
    <td>
        <select id="srhUserType" class="easyui-combobox" data-options="panelHeight:'auto'" style="width: 85px;" name="srhUserType">
            <option value="1" selected="selected">相关用户</option>
            <option value="2">相关邮箱</option>
        </select>
    </td>
    <td>
        <input type="text" style="width: 85px;" id="srhUserValue" name="srhUserValue" class="pagination-num"/>
    </td>
    <td align="left">
        <a href="#" id="resetDatagrid" class="easyui-linkbutton" iconcls="icon-reload">全部</a>
    </td>
</tr>
    <tr id="trAdvance">
    <td style="width: 100px; text-align: right;">项目的当前状态：</td>
    <td style="width: 100px; padding-left: 3px;">
        <select id="projectStatus" name="projectStatus"><option selected="selected" value="">请选择</option>
    <option value="1">QA测试</option>
    <option value="2">验收部署</option>
    <option value="3">预上线部署</option>
    <option value="4">正式部署</option>
    <option value="5">验收测试</option>
    <option value="6">预上线测试</option>
    <option value="7">预上线回滚</option>
    <option value="8">正式上线测试</option>
    <option value="9">正式上线回滚</option>
    <option value="10">已完成</option>
    <option value="11">上线失败</option>
    <option value="12">预上线回滚验证测试</option>
    <option value="13">正式上线回滚验证测试</option>
    </select>
        </td>
        <td style="width: 70px;">所属系统：</td>
        <td style="width: 100px; padding-left: 3px;">
            <select id="projectModule" name="projectModule"><option selected="selected" value="">请选择</option>
    <option value="1">Platform</option>
    <option value="2">MZ</option>
    <option value="3">JOBSEEKER</option>
    <option value="4">RD</option>
    <option value="5">Data</option>
    <option value="6">OTHERS</option>
    <option value="7">XZ_B</option>
    <option value="8">XZ_F</option>
    <option value="9">CRM</option>
    <option value="10">EDM</option>
    <option value="11">System</option>
    <option value="12">Survey</option>
    <option value="13">VIP</option>
    <option value="14">ERD</option>
    <option value="15">CS</option>
    <option value="16">EPIN</option>
    <option value="17">IPIN</option>
    </select>
        </td>
        <td style="width: 85px; text-align: right;">项目优先级：</td>
        <td style="width: 90px; padding-left: 3px;">
            <select id="projectPriority" name="projectPriority"><option selected="selected" value="">请选择</option>
    <option value="1">紧急</option>
    <option value="2">高</option>
    <option value="3">中</option>
    <option value="4">低</option>
    </select>
        </td>
        <td style="width: 80px; text-align: right;">升级包等级：</td>
        <td style="width: 80px;">
            <select id="packageDegree" name="packageDegree"><option selected="selected" value="">请选择</option>
    <option value="1">一级</option>
    <option value="2">二级</option>
    <option value="3">三级</option>
    <option value="4">四级</option>
    <option value="5">五级</option>
    </select>
        </td>
        <td style="width: 90px;">上线审批状态：</td>
        <td style="width: 90px;">
            <select id="toLineStatus" name="toLineStatus"><option selected="selected" value="">请选择</option>
    <option value="0">未处理</option>
    <option value="1">审批通过</option>
    <option value="2">审批未通过</option>
    <option value="3">审批中</option>
    </select>
        </td>
        <td><a onclick="datagridReload()" href="#" class="easyui-linkbutton" iconcls="icon-search">搜索 </a></td>
    </tr>
</table>
  
<table id="list_data" cellspacing="0" cellpadding="0">  
       <thead>  
        <tr>  
            <th field="DepartName" width="100">部门</th>  
            <th field="SiteName" width="100">网站</th>  
            <th field="SiteName" width="100">名称</th>  
            <th field="AdminName" width="100">管理员</th>  
            <th field="fldAppNote" width="100">注释</th>  
            <th field="fldAppType" width="100">类型</th>  
            <th field="Mobile" width="100">电话</th>  
            <th field="fldAppImg" width="100">职务</th>  
            <th field="fldAppMonitor" width="100">启用监测</th>  
            <th field="fldAppLevel" width="100">要重级别</th>  
        </tr>  
    </thead>  
</table>  



<script type="text/javascript">

        $(function () {
            //datagrid初始化  
            $('#list_data').datagrid({
                title: '',//应用系统列表
                iconCls: 'icon-edit',//图标  
                width: 700,
                height: 'auto',
                nowrap: false,
                striped: true,
                border: true,
                collapsible: false,//是否可折叠的  
                fit: true,//自动大小  
                url: '/ashx/List.ashx',
                border:"false",
                //sortName: 'code',  
                //sortOrder: 'desc',  
                remoteSort: false,
                idField: 'ID',
                singleSelect: false,//是否单选  
                pagination: true,//分页控件  
                rownumbers: true,//行号  
                frozenColumns: [[
                    { field: 'ck', checkbox: true }

                ]],
                toolbar: "#toolBar",
            });
            //设置分页控件  
            var p = $('#list_data').datagrid('getPager');
            $(p).pagination({
                pageSize: 10,//每页显示的记录条数，默认为10  
                pageList: [5, 10, 15],//可以设置每页记录条数的列表  
                beforePageText: '第',//页数文本框前显示的汉字  
                afterPageText: '页    共 {pages} 页',
                displayMsg: '当前显示 {from} - {to} 条记录   共 {total} 条记录',
                /*onBeforeRefresh:function(){ 
                    $(this).pagination('loading'); 
                    alert('before refresh'); 
                    $(this).pagination('loaded'); 
                }*/
            });

 });

    </script>
</body>
</html>
