# Hướng Dẫn Mở Menu Cyberpunk

## Cách 1: Sử dụng MenuController

1. Thêm `MenuController.cs` vào một GameObject trong scene chính của bạn:
   - Tạo một GameObject mới và đặt tên là "MenuController"
   - Thêm component MenuController vào GameObject này
   - Kéo prefab CyberpunkMenu vào trường "Main Menu Object"

2. Mở menu bằng phím Escape:
   - Mặc định, menu sẽ mở khi bạn nhấn phím Escape
   - Bạn có thể thay đổi phím này trong Inspector

3. Mở menu bằng code:
   ```csharp
   // Mở menu
   MenuController.Instance.OpenMenu();
   
   // Đóng menu
   MenuController.Instance.CloseMenu();
   
   // Chuyển đổi trạng thái menu (mở/đóng)
   MenuController.Instance.ToggleMenu();
   ```

## Cách 2: Tạo Menu Scene Riêng

1. Tạo một scene mới cho menu:
   - Chọn File > New Scene
   - Lưu scene với tên "MainMenuScene"
   - Sử dụng MainMenuSetup để tạo menu trong scene này

2. Thêm scene vào Build Settings:
   - Mở Build Settings (File > Build Settings)
   - Kéo scene MainMenuScene và SampleScene vào Scenes In Build
   - Đảm bảo MainMenuScene ở vị trí đầu tiên (index 0)

3. Chuyển đổi giữa các scene:
   ```csharp
   // Chuyển đến scene menu
   SceneManager.LoadScene("MainMenuScene");
   
   // Chuyển đến scene game
   SceneManager.LoadScene("SampleScene");
   ```

## Cách 3: Tạo Menu Trong Game Scene

1. Tạo menu trong scene hiện tại:
   - Trong Unity Editor, chọn Tools > Create Cyberpunk Menu
   - Menu sẽ được tạo tự động trong scene hiện tại

2. Thêm code để hiển thị/ẩn menu:
   ```csharp
   // Trong script của bạn
   public GameObject cyberpunkMenu;
   
   void Update()
   {
       if (Input.GetKeyDown(KeyCode.Escape))
       {
           cyberpunkMenu.SetActive(!cyberpunkMenu.activeSelf);
           
           // Tạm dừng/tiếp tục game
           Time.timeScale = cyberpunkMenu.activeSelf ? 0f : 1f;
       }
   }
   ```

## Lưu ý Quan Trọng

- Đảm bảo rằng bạn đã cài đặt TextMesh Pro trong project
- Nếu menu không hiển thị đúng, kiểm tra Canvas Scaler và Screen Space settings
- Để menu hoạt động tốt nhất, hãy đặt nó trong một Canvas với Render Mode là "Screen Space - Overlay"
- Nếu bạn sử dụng nhiều scene, hãy đảm bảo MenuController được đánh dấu là DontDestroyOnLoad
