function initApp() {
    const app = {
        //toast message
        familymembers: [],
        uploadedimage: "",
        selectedTerm:"",
        defaultImage: "default.jpg",
        validations: {
            FNV: "", LNV: "", ADV: "", CTV: "", CNV: "", AV: "", DOBV: "", PHV: "", APV: "", EV: "", PSV: "", CPV: ""

        },


        register: {
            FirstName: "", LastName: "", Address: "", Ward: "", City: "", Country: "", Age: "", DateOfBirth: "", Phone: "", AbroadPhone: "", Gender: "0", Email: "", Password: "", ConfirmPassword: ""
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
        validateCountry() {
            if (this.register.Country.trim().length < 3 || this.register.Country.trim().length > 50) {
                // Display an error message or perform any other validation logic
                this.validations.CNV = "Field is required";
                return false; // Validation failed
            }
            return this.validations.CNV = ""; // Validation passed

        },

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

            if (sanitizedPhone.length < 10 || sanitizedPhone.length > 20) {
                // Display an error message or perform any other validation logic
                this.validations.PHV = "Enter a valid phone number";
                return false; // Validation failed
            }

            this.register.Phone = sanitizedPhone; // Update the phone number to the sanitized value
            this.validations.PHV = ""; // Validation passed
            return true;
        },
        validateAbroadPhone() {
            const phone = this.register.Phone;
            const sanitizedPhone = phone.replace(/[^0-9()+-]/g, ''); // Remove all characters except digits, parentheses, and hyphens

            if (sanitizedPhone.length < 10 || sanitizedPhone.length > 20) {
                this.validations.APV = "Enter a valid phone number";
                return false; // Validation failed
            }

            this.register.Phone = sanitizedPhone; // Update the phone number to the sanitized value
            this.validations.APV = ""; // Validation passed
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
        initDatabase(id) {
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
                self.register.DateOfBirth = data.dateOfBirth;
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
        SaveUser() {
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
           this.validations.CPV="";

            this.validateFirstName();
            this.validateSecondName();
            this.validateAddress();
            this.validateCity();
            this.validateCountry();
            
            this.validateAge();
            this.validatedob();
           
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