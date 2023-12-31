# FPT System Training

FPT System Training là một hệ thống quản lý đào tạo nội bộ của FPT Co., được thiết kế để tối ưu hóa môi trường học tập liên tục trong tập đoàn.

Lưu ý: đây chỉ là một bài tập trong chương trình học tại FPT.

## Tác Giả
- [nhannt201](https://github.com/nhannt201)

## Hình Ảnh Tổng Quan Hệ Thống 📚

### Sơ Đồ ERD
![](https://raw.githubusercontent.com/nhannt201/FPTSystemTraining/main/Images/ERD.png)

### Active Diagram Cho Administrator
![](https://raw.githubusercontent.com/nhannt201/FPTSystemTraining/main/Images/AD_Admin.png)

### Active Diagram Cho Training Staff
![](https://raw.githubusercontent.com/nhannt201/FPTSystemTraining/main/Images/AD_TS.png)

### Active Diagram Cho Trainer
![](https://raw.githubusercontent.com/nhannt201/FPTSystemTraining/main/Images/AD_T.png)

### Tài Khoản Demo
![](https://raw.githubusercontent.com/nhannt201/FPTSystemTraining/main/Images/Login_demo.png)

## Chức Năng Chính
- Quản lý tài khoản học viên
- Quản lý giảng viên
- Quản lý danh mục khóa học
- Quản lý khóa học
- Quản lý chủ đề
- Phân công chủ đề cho khóa học
- Phân công giảng viên cho chủ đề
- Phân công học viên cho khóa học

## Các Vai Trò
1. **Administrator:**
   - Quản lý toàn bộ hệ thống.
   
2. **Training Staff:**
   - Quản lý hoạt động hàng ngày của hệ thống.

3. **Trainer:**
   - Truy cập để giảng dạy các khóa học.

## Cài Đặt - Cấu Hình Kết Nối

1. Mở tệp `web.config` trong thư mục dự án.
2. Tìm đoạn mã sau:

    ```xml
    <connectionStrings>
        <add name="dbFPTSystem" connectionString="data source=TRUNGNHAN;initial catalog=FPTSystem;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework" providerName="System.Data.SqlClient" />
    </connectionStrings>
    ```

3. Thay đổi thông tin kết nối như sau:

    ```xml
    <connectionStrings>
        <add name="dbFPTSystem" connectionString="Data Source=<SERVER_NAME>;Initial Catalog=FPTSystem;Integrated Security=True;MultipleActiveResultSets=True;App=EntityFramework" providerName="System.Data.SqlClient" />
    </connectionStrings>
    ```

    Thay thế `<SERVER_NAME>` bằng tên máy chủ cơ sở dữ liệu của bạn. Nếu bạn đang sử dụng xác thực SQL Server thay vì xác thực Windows, bạn cũng có thể cần thay đổi thông tin xác thực (User ID và Password) trong chuỗi kết nối.

4. Lưu tệp `web.config`.

### Chạy Ứng Dụng
1. Cài đặt các phần mềm cần thiết (ví dụ: .NET SDK, Node.js).
2. Mở terminal trong thư mục dự án và chạy lệnh sau để cài đặt các gói cần thiết:

    ```bash
    dotnet restore
    ```

3. Chạy lệnh sau để khởi động ứng dụng:

    ```bash
    dotnet run
    ```

4. Truy cập ứng dụng qua trình duyệt web tại địa chỉ [http://localhost:5000](http://localhost:5000).



## Yêu Cầu Kỹ Thuật

Để triển khai và phát triển dự án FPTSystemTraining, bạn cần đảm bảo rằng hệ thống của bạn đáp ứng các yêu cầu kỹ thuật sau:

1. **Visual Studio:**
   - Phiên bản: Visual Studio 2019 hoặc mới hơn.
   - [Tải Visual Studio](https://visualstudio.microsoft.com/)

2. **Entity Framework:**
   - Phiên bản: Entity Framework 6 hoặc mới hơn.
   - Đảm bảo đã cài đặt bằng cách sử dụng Package Manager Console:

     ```bash
     Install-Package EntityFramework
     ```

3. **SQL Server:**
   - Phiên bản: SQL Server 2012 hoặc mới hơn.
   - Đảm bảo rằng bạn có một cơ sở dữ liệu hoặc sử dụng tệp `database.bak` để khôi phục cơ sở dữ liệu.

4. **.NET Framework:**
   - Phiên bản: .NET Framework 4.6 hoặc mới hơn.
   - [Tải .NET Framework](https://dotnet.microsoft.com/download/dotnet-framework)

5. **Git:**
   - Phiên bản: Git 2.0 hoặc mới hơn (đối với việc quản lý mã nguồn).
   - [Tải Git](https://git-scm.com/)

Lưu ý: Để cài đặt các gói phụ thuộc và khởi chạy ứng dụng, hãy thực hiện các bước hướng dẫn trong phần "Cài Đặt" của tệp `readme.md`.


## Bản Quyền

Dự án được phát hành dưới giấy phép [MIT License](LICENSE).

