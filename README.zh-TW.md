# Toolkid.GridInventory

這個儲存庫提供了利用Unity元件`Grid`，所實現的格子收納系統，靈感來自於知名遊戲**Diablo系列**的物品收納方式。

# Sharp Editor

`Sharp`允許您對物品設定其收納時的形狀與數量。

## 使用方法

1. 添加`Placeables`於收納對象
2. 以游標點擊欲設定的方格(目前尺寸上限為5×5)
3. 利用腳本調用`InventoryManager`相關方法
4. 在**Inspector**上透過`GridSystem`設定`Grid`參數

### Inventory Manager
![Manager](https://github.com/hhs456/com.toolkid.gridinventory/blob/main/Description/Inspector.jpg)

### Grid System
![Settings](https://github.com/hhs456/com.toolkid.gridinventory/blob/main/Description/GridSettings.jpg)

## 結果

### 8x8
![8x8](https://github.com/hhs456/com.toolkid.gridinventory/blob/main/Description/Result_8x8.gif)

### 6x6
![6x6](https://github.com/hhs456/com.toolkid.gridinventory/blob/main/Description/Result_6x6.gif)