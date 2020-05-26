<?php

$username = $_POST["username"];
$password = $_POST["password"];

$conn = mysqli_connect('localhost', 'root','t1fkthsl', 'vity-game');
//check that connection happend
if(mysqli_connect_errno())
{
    echo "1: connection failed"; // error code #1 = connection failed
    exit();
}

$sql = "SELECT username,hash,salt,login FROM player WHERE username ='".$username."'";
$result = $conn->query($sql) or die("2 : Name check query failed");

if($result->num_rows > 0){
  while($row = $result->fetch_assoc()) {
    //echo $row["hash"];
    $salt = $row["salt"];
    $hash = $row["hash"];
    $login = $row["login"];

    if($login < 1){
      $loginhash = crypt($password,$salt);
      if($hash == $loginhash){
        echo "successfully";
        $sql2 = "UPDATE player SET login=(1) WHERE username ='".$username."'";
        $result2 = $conn->query($sql2);
      }
      else{
        echo "Passwords do not match";
        exit();
      }
    }
    else if($login > 0){
      echo "This User is already login game";
    }

  }
}
else{
  echo"User not found";
}


?>
