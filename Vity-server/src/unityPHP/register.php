<?php

$username = $_POST["username"];
$password = $_POST["password"];
$email = $_POST["email"];
$salt = "\$5\$rounds=5000\$" . "steamedhams" . $username. "\$";
$hash = crypt($password, $salt);

$conn = mysqli_connect('localhost', 'root','t1fkthsl', 'vity-game');
//check that connection happend
if(mysqli_connect_errno())
{
    echo "1: connection failed"; // error code #1 = connection failed
    exit();
}



$sql = "SELECT username FROM player WHERE username ='".$username."'";
$result = $conn->query($sql);

if($result->num_rows > 0){
  while($row = $result->fetch_assoc()) {
    echo "Username is already taken";
  }
}else{
  echo "Creating user ... ";
  $sql2 = "INSERT INTO player (username,hash,salt,email)
  VALUES ('".$username."','".$hash."','".$salt."','".$email."')";
  if($conn->query($sql2)===TRUE) {
    echo "New record created successfully";

    $sql3 = "INSERT INTO item (username)
    VALUES ('".$username."')";
    if($conn->query($sql3)===TRUE) {
    }else{
      echo "Error: " .$sql3 . "<br>" . $conn->error;
    }

  }else{
    echo "Error: " .$sql2 . "<br>" . $conn->error;
  }
}


$conn->close();

?>
