## Toolkid.Bounds

這個儲存庫提供了一系列關於Bounds的擴充方法，包含Collider、Renderer、Mesh等。

### Collider Maker

`Collider Maker`允許您對複數模型添加一個整合碰撞器。

#### 使用方法

1. 在欲產生碰撞的對象上加入`Collider Maker`
2. 將包含所有模型的對象拖曳至`Meshes Base`
3. 點擊欲生成的碰撞器樣式(`Box`或`Sphere`)

![Inspector](https://github.com/hhs456/Toolkid.BoundsUtility/blob/main/Description/inspector.jpg)

#### 結果

1. Box

![Multi Cube](https://github.com/hhs456/Toolkid.BoundsUtility/blob/main/Description/multiCube.jpg)

2. Sphere (Inside)

![Inside Ball](https://github.com/hhs456/Toolkid.BoundsUtility/blob/main/Description/insideBall.jpg)

3. Sphere (Outside)

![Outside Ball](https://github.com/hhs456/Toolkid.BoundsUtility/blob/main/Description/outsideBall.jpg)

### 注意事項

目前僅對`rotation`為(0,0,0)的對象有效，請小心使用！
