<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="html" encoding="utf-8"/>
  <xsl:param name="param1"></xsl:param>
  <xsl:param name="param2"></xsl:param>
  <xsl:key name="edu" match="education_experience" use="school_name/Text"/>

  <!--定义全局变量-->
  <xsl:variable name="color" select="'red'" />
  
  <xsl:template match="/">
    <html>
      <head>
        <meta http-equiv="Content-Type" content="textml; charset=utf-8" />
        <title>招商银行深圳分行2015届校园招聘应聘登记表</title>
        <style type="text/css">
          <![CDATA[
          table { border-collapse: collapse; }
          table tr td{ border: 1px solid #000;border-collapse: collapse;margin:0px;padding:0px; }
          ]]>
        </style>
      </head>
      <body>

        <xsl:for-each select="key('edu','中国人民大学')">
          <p>
            Text: <xsl:value-of select="school_name/Text"/><br />
            Value: <xsl:value-of select="school_name/Value"/><br />
            FieldType: <xsl:value-of select="school_name/FieldType"/>
          </p>
        </xsl:for-each>
        <hr/>

        <xsl:for-each select="/resume/education_experience">

          <!--规定生成哪个节点集的唯一id-->
          <xsl:value-of select="generate-id(school_name/Text)"/>
          
          <xsl:number value="position()" format="1" /> 
          <xsl:if test="position()=1">
            这是第一个 / 
          </xsl:if> 
          
          <xsl:if test="school_name/Text=''">
            未填写学校名称
          </xsl:if>
          <xsl:value-of select="school_name/Text"/>
          <p></p>
        </xsl:for-each>
        <hr/>

        <!--调用全局变量-->
        <xsl:copy-of select="$color" />
        <hr/>
        

        <p>
          <xsl:value-of select="$param1"/> - 
          <xsl:value-of select="$param2"/>
        </p>
        <p>
          姓名：<xsl:value-of select="/resume/person_info/name/Text"/>
        </p>
        <p>
          性别：<xsl:value-of select="/resume/person_info/gender/Text"/>
        </p>

        
        
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
