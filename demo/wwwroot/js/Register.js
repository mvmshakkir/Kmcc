function initApp() {
    const app = {
        //toast message
        familymembers: [],
        uploadedimage: "",
        selectedTerm: "",
        cropper: null,
        id: null,
        defaultImage: "default.jpg",
        validations: {
            FNV: "", LNV: "", ADV: "", CTV: "", CNV: "", AV: "", DOBV: "", PHV: "", APV: "", WPV: "",EV: "", PSV: "", CPV: "", PSV:""

        },


        register: {
            FirstName: "", LastName: "", Address: "", Ward: "", City: "", Country: "", Age: "", DateOfBirth: "", Phone: "", AbroadPhone: "", Whatsapp: "",Gender: "0", PravasiStatus:"", Email: "aabbcc@gmail.com", Password: "Admin@123", ConfirmPassword: "Admin@123"
        },

        validateFirstName() {
            if (this.register.FirstName.trim().length < 1 || this.register.FirstName.trim().length > 50) {
                // Display an error message or perform any other validation logic
                this.validations.FNV = "Field is required";
                return false; // Validation failed
            }
            return this.validations.FNV = ""; // Validation passed

        },
        validateSecondName() {
            if (this.register.LastName.trim().length < 1 || this.register.LastName.trim().length > 50) {
                // Display an error message or perform any other validation logic
                this.validations.LNV = "Field is required";
                return false; // Validation failed
            }
            return this.validations.LNV = ""; // Validation passed

        },
        validateAddress() {
            if (this.register.Address.trim().length < 3 || this.register.Address.trim().length > 200) {
                // Display an error message or perform any other validation logic
                this.validations.ADV = "Field is required";
                return false; // Validation failed
            }
            return this.validations.ADV = ""; // Validation passed

        },
        validateCity() {
            if (this.register.City.trim().length < 3 || this.register.City.trim().length > 50) {
                // Display an error message or perform any other validation logic
                this.validations.CTV = " Field is required ";
                return false; // Validation failed
            }
            return this.validations.CTV = ""; // Validation passed

        },
        //validateCountry() {
        //    if (this.register.Country.trim().length < 0 || this.register.Country.trim().length > 50) {
        //        // Display an error message or perform any other validation logic
        //        this.validations.CNV = "Field is required";
        //        return false; // Validation failed
        //    }
        //    return this.validations.CNV = ""; // Validation passed

        //},
        validateCountry() {
            if (!this.register.Country) {
                this.validations.CNV = "Country is required";
                return false; // Validation failed
            }
            this.validations.CNV = ""; // Clear validation message if validation passed
            return true; // Validation passed
        },
        //validatePrvasi() {
        //    if (!this.register.PravasiStatus) {
        //        this.validations.PRV = "Status is required";
        //        return false; // Validation failed
        //    }
        //    this.validations.PRV = ""; // Clear validation message if validation passed
        //    return true; // Validation passed
        //},




        validateAge() {
            if (this.register.Age.length < 1 || this.register.Age.length > 50) {
                // Display an error message or perform any other validation logic
                this.validations.AV = "Field is required";
                return false; // Validation failed
            }
            return this.validations.AV = ""; // Validation passed

        },
        validatedob() {
            if (!this.register.DateOfBirth) {
                this.validations.DOBV = "Field is required"
                return false;
            }
            return this.validations.DOBV = "";
        },
       
        validatePhone() {
            const phone = this.register.Phone;
            // Remove non-digit characters except '+' for validation
            const sanitizedPhone = phone.replace(/[^\d+]/g, '');

            if (sanitizedPhone.length < 10 || sanitizedPhone.length >= 13) {
                // Display an error message or perform any other validation logic
                this.validations.PHV = "Enter a valid phone number";
                return false; // Validation failed
            }

            this.register.Phone = sanitizedPhone; // Update the phone number to the sanitized value
            this.validations.PHV = ""; // Validation passed
            return true;
        },
        validateAbroadPhone() {
            const phone = this.register.AbroadPhone;
            const sanitizedPhone = phone.replace(/[^0-9()+-]/g, ''); // Remove all characters except digits, parentheses, and hyphens

            if (sanitizedPhone.length < 10 || sanitizedPhone.length > 15) {
                this.validations.APV = "Enter a valid phone number";
                return false; // Validation failed
            }

            this.register.AbroadPhone = sanitizedPhone; // Update the phone number to the sanitized value
            this.validations.APV = ""; // Validation passed
            return true;
        },
        validateWhatsapp() {
            const phone = this.register.Whatsapp;
            const sanitizedPhone = phone.replace(/[^0-9()+-]/g, ''); // Remove all characters except digits, parentheses, and hyphens

            if (sanitizedPhone.length < 12 || sanitizedPhone.length > 15) {
                this.validations.WPV = "Enter a valid phone number with country code";
                return false; // Validation failed
            }

            this.register.Whatsapp = sanitizedPhone; // Update the phone number to the sanitized value
            this.validations.WPV = ""; // Validation passed
            return true;
        },

        validateEmail() {
            const email = this.register.Email.trim();
            //const emailError = document.getElementById("emailError");

            // Define a regular expression pattern for email validation.
            const emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;

            if (!emailPattern.test(email)) {
                this.validations.EV = "Enter valid email";
                return false; // Validation failed
            }

            return this.validations.EV = ""; // Validation passed

        },
        
         checkEmailExists() {
            var userEmail = this.register.Phone;
            var self = this;

            var emailCheckSettings = {
                url: "/api/EmailCheck",
                method: "POST",
                timeout: 0,
                headers: {
                    "Content-Type": "application/json"
                },
                data: JSON.stringify({ Phone: userEmail })
            };

            return $.ajax(emailCheckSettings)
                .done(function (emailCheckResponse) {
                    if (emailCheckResponse.exists) {
                        self.validations.PHV = "Phone number already exists. Please use a different number.";
                    } else {
                        self.validations.PHV = ""; // Clear the validation message if email does not exist
                    }
                })
                .fail(function (error) {
                    console.error("Error checking email:", error);
                    self.validations.EV = "Error checking email. Please try again.";
                });
        },
        validatePassword() {
            console.log("ID:", this.register.Id);

            if ((this.register.Id || "") !== "") {
                return true; // Skip password validation during update
            }
            const ps = this.register.Password;
            //const passwordError = document.getElementById("passwordError");

            // Define password validation rules:
            const minLength = 6;
            const uppercaseRegex = /[A-Z]/;
            const lowercaseRegex = /[a-z]/;
            const digitRegex = /[0-9]/;
            const specialCharacterRegex = /[!@#\$%\^&\*\(\)_\+\-=\[\]\{\};:'",<>\./?\\|]/; // Add your own special characters

            // Check each requirement
            if (ps.length < minLength) {
                this.validations.PSV = "Password must be 6 letterrs";
                return false;
            }
            if (!uppercaseRegex.test(ps)) {
                this.validations.PSV = "Password must contain at least one uppercase letter.";
                return false;
            }
            if (!lowercaseRegex.test(ps)) {
                this.validations.PSV = "Password must contain at least one lowercase letter.";
                return false;
            }
            if (!digitRegex.test(ps)) {
                this.validations.PSV = "Password must contain at least one digit.";
                return false;
            }
            if (!specialCharacterRegex.test(ps)) {
                this.validations.PSV = "Password must contain at least one special character.";
                return false;
            } else {

                this.validations.PSV = ""; // Clear the error message
                return true; // Validation passed
            }
        },

        validateConformPassword() {
            console.log("ID:", this.register.Id);

            if ((this.register.Id || "") !== "") {
                return true; // Skip password validation during update
            }
            if (this.register.Password !== this.register.ConfirmPassword) {
                // Display an error message or perform any other validation logic
                this.validations.CPV = "Password must be same";
                return false; // Validation failed
            }
            return this.validations.CPV = ""; // Validation passed

        },    
        uploadFiles(inputId) {
            var input = document.getElementById(inputId);
            var files = input.files;
            var formData = new FormData();
            for (var i = 0; i != files.length; i++) {
                formData.append("files", files[i]);
            }
            var k = this;
            $.ajax(
                {
                    url: "/api/UploadFile?files" + files,
                    data: formData,
                    processData: false,
                    contentType: false,
                    type: "POST",
                    success: function (data) {
                        k.uploadedimage = data;
                    }
                }
            );
        },

       
        previewAndCropImage(inputId) {
            const input = document.getElementById(inputId);
            const file = input.files[0];

            if (file) {
                const reader = new FileReader();

                reader.onload = (e) => {
                    const image = this.$refs.imagePreview; // Reference to the image preview element
                    image.src = e.target.result;
                    image.style.display = 'block'; // Show the image

                    // Show the image container
                    const imageContainer = document.getElementById('imageContainer');
                    imageContainer.style.display = 'block';

                    if (this.cropper) {
                        this.cropper.destroy(); // Destroy previous cropper instance if it exists
                    }

                    // Initialize Cropper.js
                    this.cropper = new Cropper(image, {
                        aspectRatio: 1, // Adjust aspect ratio as needed
                        viewMode: 1,
                        ready() {
                            document.getElementById('cropButton').style.display = 'block'; // Show the crop button when Cropper is ready
                        }
                    });
                };

                reader.readAsDataURL(file);
            } else {
                // Hide the image container if no file is selected
                document.getElementById('imageContainer').style.display = 'none';
            }
        },

      
        cropAndUploadImage() {
            if (this.cropper) {
                const canvas = this.cropper.getCroppedCanvas();
                canvas.toBlob((blob) => {
                    const croppedFile = new File([blob], "cropped_image.jpg", { type: "image/jpeg" });

                    // Create a new FormData object and append the cropped image file
                    const formData = new FormData();
                    formData.append("files", croppedFile);

                    // Call the existing uploadFilesWithFormData method with the FormData
                    this.uploadFilesWithFormData(formData);

                    const imageContainer = document.getElementById('imageContainer');
                    imageContainer.style.display = 'none';
                    // Hide image preview and crop button
                    this.$refs.imagePreview.style.display = 'none';
                    document.getElementById('cropButton').style.display = 'none';

                    // Destroy cropper instance
                    this.cropper.destroy();
                }, 'image/jpeg');
            }
        },

        // Function to handle file upload with FormData
        uploadFilesWithFormData(formData) {
            $.ajax({
                url: "/api/UploadFile",
                data: formData,
                processData: false,
                contentType: false,
                type: "POST",
                success: (data) => {
                    this.uploadedimage = data; // Handle the response as needed
                },
                error: (error) => {
                    console.error('Upload failed:', error);
                }
            });
        },

        // Update uploadFiles method to use cropping
        uploadFiles(inputId) {
            this.previewAndCropImage(inputId);
        },




        initDatabase(id) {
            this.id = id;

            var getUserSettings = {
                "url": "/api/Edit?id=" + id,
                "method": "GET",


            };
            var self = this;





            $.ajax(getUserSettings).done(function (data) {
                if (id) {
                    self.register.Id = id
                }
                self.register.FirstName = data.firstName;
                self.register.LastName = data.lastName;
                self.register.Address = data.address;
                self.register.Ward = data.ward;
                self.register.City = data.city;
                self.register.Country = data.country;
                self.register.Phone = data.phone;
                self.register.Email = data.email;
                self.uploadedimage = data.userImage;
                self.register.Age = data.age;
                self.register.AbroadPhone = data.abroadPhone;
                self.register.Whatsapp = data.whatsapp;
                // self.register.DateOfBirth = data.dateOfBirth;
                self.register.DateOfBirth = formatDate(data.dateOfBirth);
                //self.register.Password = data.ConfirmPassword;
                //self.register.ConfirmPassword = data.ConfirmPassword;
                self.register.Id = data.id;
                self.familymembers = [];
                data.family.forEach(function (familyMember) {
                    var fm = {};
                    fm.familyId = familyMember.familyId;
                    fm.Name = familyMember.name;
                    fm.Age = familyMember.age;
                    fm.Relation = familyMember.relation;
                    self.familymembers.push(fm);
                });


            });
        },

        
        addmember() {
            this.familymembers.push({ Name: "", Age: "", Relation: "" });
            alert(familymembers);
        },
        deletemember(member) {
            this.familymembers.splice(this.familymembers.indexOf(member), 1);
        },
        async SaveUser() {
          this.validations.FNV = "";
          this.validations.LNV = "";
          this.validations.CNV = "";
          this.validations.CTV = "";
          this.validations.ADV = "";
           
            this.validations.AV = "";
            this.validations.DOBV = "";
            this.validations.PHV = "";
            this.validations.APV = "";
          this.validations.EV="";
          this.validations.PSV="";
            this.validations.CPV = "";
            this.validations.WPV = "";

            //this.validatePrvasi();
            this.validateFirstName();
            this.validateSecondName();
            this.validateAddress();
            this.validateCity();
            this.validateCountry();
            
            this.validateAge();
            this.validatedob();
            this.validateWhatsapp();
            this.validatePhone();
            this.validateAbroadPhone();
            this.validateEmail();
            this.validatePassword();
            this.validateConformPassword();
            // Check for errors in the validations object
            if (
                this.validations.FNV ||
                this.validations.LNV ||
                this.validations.CNV ||
                this.validations.CTV ||
                this.validations.ADV ||

                this.validations.WPV ||
                this.validations.AV  ||
                this.validations.DOBV||
                this.validations.PHV ||
                this.validations.APV ||
                this.validations.EV  ||
                this.validations.PSV ||
                this.validations.CPV

                // Add more conditions for other fields as needed
            ) {
                // Display error messages or perform actions based on errors
                return; // Validation failed, do not proceed
            }
       
            //var id = this.id;
            //await this.checkEmailExists();

            //if (this.validations.EV) {
            //    return; // Email exists, stop further processing
            //}
            if (!this.id) {
                await this.checkEmailExists();
                if (this.validations.PHV) {
                    return; // Email exists, stop further processing
                }
            }


            if ((this.uploadedimage||"")!=""){
                this.register.UserImage=this.uploadedimage;
            }else{
                 this.register.UserImage=this.defaultImage;

            }
            
            this.register.family = this.familymembers;
            var registerData = this.register;

            var jdate = JSON.stringify(registerData);
            var settings = {
                "url": "/api/UserRegistar", 
                "method": "POST",
                "timeout": 0,
                "headers": {
                    "Content-Type": "application/json"
                    
                    
                },
                "data": jdate
            }

            $.ajax(settings).done(function (response) {
                console.log(response);
                var kdate = JSON.stringify(registerData);
                console.log(kdate);
                var parsedData = JSON.parse(kdate);
                var username = parsedData.Email; 

             window.location.href = '/Profile/viewprofile';
                
            });
           
            
            ;
}
    }
    return app;
}


function formatDate(dateString) {
    const date = new Date(dateString);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
}


