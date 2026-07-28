# RelativeSource Binding

| 속성              | 내용                                   |
| :---------------- | :------------------------------------- |
| `AncestorType`  | 상위 컨트롤 중 참조할 항목             |
| `AncestorLevel` | 상위 컨트롤 참조할 항목 중 몇번째 인지 |
| `Path`          | 어떤 속성의 값을 참조할 것인지?        |

### 예시

| 구분      | 내용                                                                                                           |
| :-------- | :------------------------------------------------------------------------------------------------------------- |
| 예시구문1 | `Text="{Binding RelativeSource={RelativeSource AncestorType=StackPanel, AncestorLevel=1}, Path=Background}"` |
| 해석1     | 바로 상위에 있는 StackPanel의 배경색상을 참조할 것                                                             |
| 예시구문2 | `Text="{Binding RelativeSource={RelativeSource AncestorType=StackPanel, AncestorLevel=2}, Path=Background}"` |
| 해석2     | 상위 두번째에 있는 StackPanel의 배경색상을 참조할 것                                                           |
