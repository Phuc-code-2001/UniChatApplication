// Đối tượng `Validator`
function Validator(options) {
  function getParent(element, selector) {
    while (element.parentElement) {
      if (element.parentElement.matches(selector)) {
        return element.parentElement;
      }
      element = element.parentElement;
    }
  }

  var selectorRules = {};

  // Hàm thực hiện validate
  function validate(inputElement, rule) {
    var errorElement = getParent(
      inputElement,
      options.formGroupSelector
    ).querySelector(options.errorSelector);
    var errorMessage;

    // Lấy ra các rules của selector
    var rules = selectorRules[rule.selector];

    // Lặp qua từng rule & kiểm tra
    // Nếu có lỗi thì dừng việc kiểm
    for (var i = 0; i < rules.length; ++i) {
      switch (inputElement.type) {
        case "radio":
        case "checkbox":
          errorMessage = rules[i](
            formElement.querySelector(rule.selector + ":checked")
          );
          break;
        default:
          errorMessage = rules[i](inputElement.value);
      }
      if (errorMessage) break;
    }

    if (errorMessage) {
      errorElement.innerText = errorMessage;
      getParent(inputElement, options.formGroupSelector).classList.add(
        "invalid"
      );
    } else {
      errorElement.innerText = "";
      getParent(inputElement, options.formGroupSelector).classList.remove(
        "invalid"
      );
    }

    return !errorMessage;
  }

  // Lấy element của form cần validate
  var formElement = document.querySelector(options.form);
  if (formElement) {
    // Khi submit form
    formElement.onsubmit = function (e) {
      e.preventDefault();

      var isFormValid = true;

      // Lặp qua từng rules và validate
      options.rules.forEach(function (rule) {
        var inputElement = formElement.querySelector(rule.selector);
        var isValid = validate(inputElement, rule);
        if (!isValid) {
          isFormValid = false;
        }
      });

      if (isFormValid) {
        // Trường hợp submit với javascript
        if (typeof options.onSubmit === "function") {
          var enableInputs = formElement.querySelectorAll("[name]");
          var formValues = Array.from(enableInputs).reduce(function (
            values,
            input
          ) {
            switch (input.type) {
              case "radio":
                values[input.name] = formElement.querySelector(
                  'input[name="' + input.name + '"]:checked'
                ).value;
                break;
              case "checkbox":
                if (!input.matches(":checked")) {
                  values[input.name] = "";
                  return values;
                }
                if (!Array.isArray(values[input.name])) {
                  values[input.name] = [];
                }
                values[input.name].push(input.value);
                break;
              case "file":
                values[input.name] = input.files;
                break;
              default:
                values[input.name] = input.value;
            }

            return values;
          },
            {});
          options.onSubmit(formValues);
        }
        // Trường hợp submit với hành vi mặc định
        else {
          formElement.submit();
        }
      }
    };

    // Lặp qua mỗi rule và xử lý (lắng nghe sự kiện blur, input, ...)
    options.rules.forEach(function (rule) {
      // Lưu lại các rules cho mỗi input
      if (Array.isArray(selectorRules[rule.selector])) {
        selectorRules[rule.selector].push(rule.test);
      } else {
        selectorRules[rule.selector] = [rule.test];
      }

      var inputElements = formElement.querySelectorAll(rule.selector);

      Array.from(inputElements).forEach(function (inputElement) {
        // Xử lý trường hợp blur khỏi input
        inputElement.onblur = function () {
          validate(inputElement, rule);
        };

        // Xử lý mỗi khi người dùng nhập vào input
        inputElement.oninput = function () {
          var errorElement = getParent(
            inputElement,
            options.formGroupSelector
          ).querySelector(options.errorSelector);
          errorElement.innerText = "";
          getParent(inputElement, options.formGroupSelector).classList.remove(
            "invalid"
          );
        };
      });
    });
  }
}

// Định nghĩa rules
// Nguyên tắc của các rules:
// 1. Khi có lỗi => Trả ra message lỗi
// 2. Khi hợp lệ => Không trả ra cái gì cả (undefined)
Validator.isRequired = function (selector, message) {
  return {
    selector: selector,
    test: function (value) {
      return value ? undefined : message || "Input can not be blank!";
    },
  };
};

//check email có đuôi @fpt.edu.vn
Validator.isEmail = function (selector, message) {
  return {
    selector: selector,
    test: function (value) {
      var regex = /^\w+([\.-]?\w+)*@*(fpt\.edu\.vn)+$/;

      return regex.test(value)
        ? undefined
        : message || "Please correct email format: email@fpt.edu.vn";
    },
  };
};

//check email basic for customer
Validator.isEmailBasic = function (selector, message) {
    return {
        selector: selector,
        test: function (value) {
            var regex = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

            return regex.test(value)
                ? undefined
                : message || "Please correct email format: sample@gmail.com";
        },
    };
};

//check Student code  gồm 2 chữ cái + 6 chữ số
Validator.isStuCode = function (selector, message) {
  return {
    selector: selector,
    test: function (value) {
      var regex = /^[a-zA-Z]{2}(\d{6})$/;

      return regex.test(value)
        ? undefined
        : message ||
        "Please only enter in the standard form: 2 letters + 6 digits";
    },
  };
};

//check teacher code  gồm 8 chữ số
Validator.isTeacherCode = function (selector, message) {
  return {
    selector: selector,
    test: function (value) {
      var regex = /^(\d{8})$/;

      return regex.test(value)
        ? undefined
        : message || "Please enter only 8 digits!";
    },
  };
};

//Full name chỉ gồm chữ cái
Validator.isName = function (selector, message) {
  return {
    selector: selector,
    test: function (value) {
      var regex = /^[^0-9_!¡?÷?¿/\\+=@#$%ˆ&*(){}|~<>;:[\]]{2,}$/;

      return regex.test(value)
        ? undefined
        : message || "Please enter only letters";
    },
  };
};

//Class có 2 kí tự đầu là chữ cái và 4 kí tự sau là số 0-9
Validator.isNameClass = function (selector, message) {
  return {
    selector: selector,
    test: function (value) {
      var regex = /^[a-zA-Z]{2}[0-9]{4}$/;

      return regex.test(value)
        ? undefined
        : message || "Please only enter in the standard form: 2 letters + 4 digits";
    },
  };
};

//Subject code có 3 kí tự đầu là chữ cái và 3 kí tự sau là số 0-9
Validator.isVNphone= function (selector, message) {
  return {
    selector: selector,
    test: function (value) {
        var regex = /^(03|05|07|08|09|01[2|6|8|9])+([0-9]{8})\b$/;

      return regex.test(value)
        ? undefined
        : message || "Please only enter VN phone";
    },
  };
};

//check input có đúng số lượng kí tự theo tham số truyền vào
Validator.minLength = function (selector, min, message) {
  return {
    selector: selector,
    test: function (value) {
      return value.length == min
        ? undefined
          : message || `Please enter ${min} characters correctly`;
    },
  };
};

//giới hạn số kí tự cho input
Validator.maxLength = function (selector, max, message) {
  return {
    selector: selector,
    test: function (value) {
      return value.length <= max
        ? undefined
        : message || `Please enter max ${max} characters`;
    },
  };
};

//Check VNphone
Validator.isSubCode = function (selector, message) {
    return {
        selector: selector,
        test: function (value) {
            var regex = /^[a-zA-Z]{3}[0-9]{3}$/;

            return regex.test(value)
                ? undefined
                : message || "Please only enter in the standard form: 3 letters + 3 digits";
        },
    };
};

//confirm lại giá trị nhập vào
Validator.isVNphone = function (selector, getConfirmValue, message) {
  return {
    selector: selector,
    test: function (value) {
      return value === getConfirmValue()
        ? undefined
        : message || "Giá trị nhập vào không chính xác";
    },
  };
};

