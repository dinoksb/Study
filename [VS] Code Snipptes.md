# [Visual Studio] Code Snippets

코드 조각은 상황에 맞는 메뉴 명령이나 바로 가기 키 조합을 사용하여 코드 파일에 삽입할 수 있는 다시 사용 가능한 작은 블록입니다.
일반적으로 `try-finally` 또는 `if-else` 블록과 같이 자주 사용되는 코드 블록을 포함하지만 전체 클래스나 메서드를 삽입하는 데 사용할 수 있습니다.

## Code Snippet Templete

기본 조각 코드 템플릿

```xml
<?xml version="1.0" encoding="utf-8"?>
<CodeSnippets
    xmlns="http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet">
    <CodeSnippet Format="1.0.0">
        <Header>
            <Title></Title>
        </Header>
        <Snippet>
            <Code Language="">
                <![CDATA[]]>
            </Code>
        </Snippet>
    </CodeSnippet>
</CodeSnippets>
```
[MSDN - Creating a Code Snippet][ccs]

[ccs]: https://msdn.microsoft.com/en-us/library/ms165394.aspx

## Code Snippet Sample

기본 조각 템플릿 샘플 코드

```xml
<?xml version="1.0" encoding="utf-8"?>
<CodeSnippets>
<CodeSnippet>
<Header>
<Title>ctor</Title>
<Shortcut>ctor</Shortcut>
</Header>
<Snippet>
<Declarations>
    <Literal Editable="false"><ID>classname</ID><Function>ClassName()</Function></Literal>
</Declarations>
<Code><![CDATA[public $classname$($end$){}]]></Code>
</Snippet>
</CodeSnippet>
</CodeSnippets>
```

