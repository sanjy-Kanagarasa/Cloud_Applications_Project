
Vagrant.configure(2) do |config| 
  config.vm.box = "ubuntu/xenial64"
  config.vm.hostname = "B.Ela-Photography"

  config.vm.network "private_network", ip: "192.168.137.50"
  config.vm.network :forwarded_port, guest: 10000, host: 10000, auto_correct: true
config.vm.network :forwarded_port, guest: 8000, host: 8000, auto_correct: true
config.vm.network :forwarded_port, guest: 587, host: 587, auto_correct: true
config.vm.network :forwarded_port, guest: 25, host: 25, auto_correct: true


end