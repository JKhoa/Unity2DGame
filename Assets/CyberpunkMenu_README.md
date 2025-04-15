# Cyberpunk-Style Menu Implementation

This package adds a Cyberpunk 2077-inspired main menu to your GrapplerShooting game, with English text instead of Polish as shown in the reference image.

## How to Implement

### Method 1: Using the Editor Script

1. Open your Unity project
2. In the Unity menu, go to **Tools > Create Cyberpunk Menu**
3. The menu will be automatically created in your current scene

### Method 2: Manual Setup

1. Create a new scene or open your existing menu scene
2. Create an empty GameObject and name it "CyberpunkMenu"
3. Add the following components to it:
   - `MainMenuManager.cs`
   - `CyberpunkMenuStyle.cs`
4. Set up the menu hierarchy as follows:
   ```
   CyberpunkMenu
   ├── BackgroundPanel (Image)
   ├── GameTitle (TextMeshProUGUI)
   ├── MenuItems
   │   ├── ContinueButton
   │   ├── NewGameButton
   │   ├── LoadGameButton
   │   ├── SettingsButton
   │   ├── CreditsButton
   │   └── ExitGameButton
   └── VersionText (TextMeshProUGUI)
   ```
5. Configure the `MainMenuManager` component by assigning the buttons to their respective fields

## Menu Items

The English menu items are:
- CONTINUE
- NEW GAME
- LOAD GAME
- SETTINGS
- CREDITS
- EXIT GAME

## Customization

You can customize the menu appearance by modifying the properties in the `CyberpunkMenuStyle` component:

- **Colors**: Change the primary, secondary, background, and text colors
- **Fonts**: Assign custom fonts for the title and menu items
- **Effects**: Enable/disable the glitch effect and adjust its parameters

## Integration with Your Game

To integrate the menu with your game:
1. Update the button actions in `MainMenuManager.cs` to load your game scenes
2. Modify the styling in `CyberpunkMenuStyle.cs` to match your game's visual theme
3. Add your game's logo or title text

## Notes

- The menu is designed to mimic the Cyberpunk 2077 style with a dark background, yellow/red accent colors, and a minimalist layout
- The glitch effect adds a cyberpunk aesthetic by occasionally distorting menu text
- Make sure you have TextMesh Pro installed in your project for proper text rendering

## Hướng dẫn sử dụng Cyberpunk Menu

### Cách thêm menu vào game

#### Cách 1: Sử dụng Editor Menu
1. Trong Unity Editor, chọn GameObject > UI > Cyberpunk Menu
2. Menu sẽ tự động được thêm vào scene hiện tại

#### Cách 2: Thêm thủ công
1. Tạo một GameObject mới trong scene
2. Đặt tên là "CyberpunkMenu"
3. Thêm component EmergencyMenuButton vào GameObject

## Cách sử dụng menu trong game

1. **Mở menu:**
   - Nhấn nút MENU ở góc trên bên phải màn hình
   - Hoặc nhấn phím M trên bàn phím

2. **Đóng menu:**
   - Nhấn nút CONTINUE trong menu
   - Hoặc nhấn lại phím M

3. **Các tùy chọn menu:**
   - CONTINUE: Tiếp tục game
   - NEW GAME: Bắt đầu game mới
   - LOAD GAME: Tải game đã lưu
   - SETTINGS: Mở cài đặt
   - CREDITS: Xem credits
   - EXIT GAME: Thoát game

## Tùy chỉnh menu

Bạn có thể tùy chỉnh menu bằng cách chọn GameObject CyberpunkMenu trong Hierarchy và điều chỉnh các thông số trong Inspector:

- **Show In Top Right:** Hiển thị nút ở góc trên bên phải (true) hoặc góc trên bên trái (false)
- **Button Width/Height:** Kích thước nút
- **Margin:** Khoảng cách từ mép màn hình
- **Menu Key:** Phím tắt để mở menu (mặc định là M)

## Lưu ý

- Menu sẽ tự động tạm dừng game khi mở
- Menu sử dụng IMGUI nên sẽ hoạt động trong mọi trường hợp, không phụ thuộc vào Canvas
- Nếu bạn muốn thay đổi giao diện menu, hãy chỉnh sửa file EmergencyMenuButton.cs
